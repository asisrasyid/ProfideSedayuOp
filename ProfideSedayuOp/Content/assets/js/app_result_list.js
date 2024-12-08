/**
 * App Invoice List (jquery)
 */

'use strict';

$(function () {
    // Variable declaration for table
    var dt_invoice_table = $('.invoice-list-table');

    // Invoice datatable
    var dt_invoice = dt_invoice_table.DataTable({
        //Ambil data 
        ajax:
        {
            url: assetsPath + 'json/BackDataDone.json',
            type: 'GET', // Menggunakan metode POST untuk mengirim parameter
            //data: function (d) {
            //    // Menambahkan parameter ke permintaan AJAX
            //    d.User_Pengaju = '';
            //    d.User_Group = '';
            //    d.tipw = '';
            //    d.status = '40';
            //    d.startDate = '';
            //    d.endDate = '';
            //},
            dataSrc: function (json) {
                console.log('Data JSON:', json); // Tampilkan data
                return json;
            }
        },
        columns: [
            // Declare data yang di ambil dari server
            { data: 'NoTransaksi' },
            { data: 'cabang' },
            { data: 'Nama_PT' },
            { data: 'TipeTransaksi_CekingDesc' },
            { data: 'User_Pengaju' },
            { data: 'Wilayah_PT' },
            {
                data: 'Tgl_Pengajuan',
                render: function (data) {
                    return moment(data).format('DD-MM-YYYY');
                }
            },
            { data: 'Jenis_Badan' },
            { data: 'cabang' },
        ],
        columnDefs: [
            {
                // For Responsive
                className: 'control',
                responsivePriority: 2,
                searchable: false,
                targets: 0,
                render: function (data, type, full, meta) {
                    return '';
                }
            },

            {
                // button
                targets: 1,
                orderable: false,
                checkboxes: {
                    selectAllRender: '<button class="btn btn-success"><i class="ri-download-cloud-2-line ri"></i></button> '
                },
                render: function (data, type, full, meta) {
                    var $no_trns = full['NoTransaksi'];
                    var $wilayah = full['Wilayah_PT'];
                    var $nama = full['Nama_PT'];
                    var $user = full['User_Pengaju'];
                    var $cabang = full['cabang'];
                    var $file = full['File_pdf'];
                    var $TanggalPengajuan = new Date(full['Tgl_Pengajuan']);
                    return `<button  class="download-link btn btn-success" data-file="${$file}" data-no="${$no_trns}" data-nama="${$nama}" data-user="${$user}" data-wilayah="${$wilayah}" data-cabang = "${$cabang}" data-tanggal= "${moment($TanggalPengajuan).format('YYYY-MM-DD')}"><i class="ri-download-cloud-2-line"></i></button>`;
                },
                searchable: false
            },
            {
                // Kolom No Transaksi
                targets: 2,
                render: function (data, type, full, meta) {
                    var $invoice_id = full['NoTransaksi'];
                    // Creates full output for row
                    var $row_output = '<a href="app-invoice-preview.html"><span>' + $invoice_id + '</span></a>';
                    return $row_output;
                }
            },
            {
                // Data Cek Kolom
                targets: 4,
                responsivePriority: 4,
                render: function (data, type, full, meta) {
                    var $name = full['Nama_PT'],
                        $email = full['Wilayah_PT'],
                        $image = false,
                        $rand_num = Math.floor(Math.random() * 11) + 1,
                        $user_img = $rand_num + '.png';
                    if ($image === true) {
                        // For Avatar image
                        var $output =
                            '<img src="' + assetsPath + 'img/avatars/' + $user_img + '" alt="Avatar" class="rounded-circle">';
                    } else {
                        // For Avatar badge
                        var stateNum = Math.floor(Math.random() * 6),
                            states = ['success', 'danger', 'warning', 'info', 'dark', 'primary', 'secondary'],
                            $state = states[stateNum],
                            $name = full['Nama_PT'],
                            $initials = ($name || '').match(/\b\w/g) || [];
                        $initials = (($initials.shift() || '') + ($initials.pop() || '')).toUpperCase();
                        $output = '<span class="avatar-initial rounded-circle bg-label-' + $state + '">' + $initials + '</span>';
                    }
                    // Creates full output for row
                    var $row_output =
                        '<div class="d-flex justify-content-start align-items-center">' +
                        '<div class="avatar-wrapper">' +
                        '<div class="avatar avatar-sm me-3">' +
                        $output +
                        '</div>' +
                        '</div>' +
                        '<div class="d-flex flex-column gap-50">' +
                        '<a href="pages-profile-user.html" class="text-truncate text-heading fw-medium"><p class="mb-0">' +
                        $name +
                        '</p></a>' +
                        '<small class="text-truncate">' +
                        $email +
                        '</small>' +
                        '</div>' +
                        '</div>';
                    return $row_output;
                }
            },
            {
                // User pengaju kolom
                targets: 5,
                render: function (data, type, full, meta) {
                    var $total = full['User_Pengaju'].toUpperCase() || '';
                    return '<span class="d-none">' + $total + '</span>' + $total;
                }
            },
            {
                // Tanggal Pengajuan
                targets: 6,
                render: function (data, type, full, meta) {
                    var $due_date = new Date(full['Tgl_Pengajuan']);
                    // Creates full output for row
                    var $row_output =
                        '<span class="d-none">' +
                        moment($due_date).format('YYYYMMDD') +
                        '</span>' +
                        moment($due_date).format('DD MMM YYYY');
                    $due_date;
                    return $row_output;
                }
            },
            {
                // Cabang (data belum ada sampel pakau data user pengaju)
                targets: 7,
                orderable: true,
                render: function (data, type, full, meta) {
                    var $balance = full['cabang'];
                    if ($balance === 0) {
                        var $badge_class = 'bg-label-success';
                        return '<span class="badge rounded-pill ' + $badge_class + '"> Paid </span>';
                    } else {
                        return '<span class="text-heading">' + $balance + '</span>';
                    }
                }
            },
            {
                targets: 8,
                visible: false
            },
        ],
        order: [[6, 'asc']],
        dom:
            '<"row ms-2 me-3"' +
            '<"col-12 col-md-6 d-flex align-items-center justify-content-center justify-content-md-start ps-3 gap-4"l<"dt-action-buttons text-xl-end text-lg-start text-md-end text-start"B>>' +
            '<"col-12 col-md-6 d-flex align-items-center justify-content-end flex-column flex-md-row pe-3 gap-md-4 mt-n5 mt-md-0"f<"invoice_status mb-5 mb-md-0">>' +
            '>t' +
            '<"row mx-1"' +
            '<"col-sm-12 col-md-6"i>' +
            '<"col-sm-12 col-md-6"p>' +
            '>',
        language: {
            sLengthMenu: 'Show _MENU_',
            search: '',
            searchPlaceholder: 'Cari Data'
        },
        lengthMenu: [[5, 10, 50, 100, 1000, -1], [5, 10, 50, 100, 1000, "MAX"]],
        pageLength: 10,
        buttons: [
            {
                text: '<i class="ri-add-line ri-16px me-md-1_5"></i><span class="d-md-inline-block d-none">Buat Invoice</span>',
                className: 'btn btn-success waves-effect waves-light',
                action: function (e, dt, button, config) {
                    window.location = 'app_invoice_select';
                }
            }
        ],
        scrollX: true, // Mengaktifkan scroll horizontal
        scrollY: '420px', // Mengatur tinggi tabel agar scrollable secara vertikal
        scroller: true, // Opsional, untuk performa lebih baik pada tabel besar
        // For responsive popup
        responsive: {
            details: {
                display: $.fn.dataTable.Responsive.display.modal({
                    header: function (row) {
                        var data = row.data();
                        return 'Details of ' + data['Nama_PT'];
                    }
                }),
                type: 'column',
                renderer: function (api, rowIdx, columns) {
                    var data = $.map(columns, function (col, i) {
                        return col.title !== '' // ? Do not show row in modal popup if title is blank (for check box)
                            ? '<tr data-dt-row="' +
                            col.rowIndex +
                            '" data-dt-column="' +
                            col.columnIndex +
                            '">' +
                            '<td>' +
                            col.title +
                            ':' +
                            '</td> ' +
                            '<td>' +
                            col.data +
                            '</td>' +
                            '</tr>'
                            : '';
                    }).join('');

                    return data ? $('<table class="table"/><tbody />').append(data) : false;
                }
            }
        },
        initComplete: function () {
            // Adding role filter once table initialized
            this.api()
                .columns(8)
                .every(function () {
                    var column = this;
                    var select = $(
                        '<select id="UserRole" class="form-select form-select-sm"><option value=""> Cabang </option></select>'
                    )
                        .appendTo('.invoice_status')
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });

                    column
                        .data()
                        .unique()
                        .sort()
                        .each(function (d, j) {
                            select.append('<option value="' + d + '" class="text-capitalize">' + d + '</option>');
                        });
                });
        }
    });


    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var startDate = $('#start-date').val();
            var endDate = $('#end-date').val();

            // Kolom tanggal (sesuaikan index)
            var dateColumn = data[6];
            var convertedDate = moment(dateColumn, 'DD-MM-YYYY').format('YYYY-MM-DD');

            // Validasi rentang tanggal
            if (startDate && endDate) {
                return moment(convertedDate).isBetween(startDate, endDate, null, '[]');
            }
            return true;
        }
    );
    $(document).on('click', '.download-link', function () {
        $.blockUI({
            message: '<div class="spinner-border text-primary" role="status"></div>',
            css: {
                backgroundColor: 'transparent',
                border: '0'
            },
            overlayCSS: {
                backgroundColor: '#fff',
                opacity: 0.8
            }
        });
        var filedown = $(this).data("file");
        var Namefile = $(this).data("nama").toUpperCase();
        var NameUser = $(this).data("user");
        $.ajax({
            url: '/Home/GetDataPengecekan', // Ganti dengan endpoint server Anda
            method: 'POST',
            dataType: 'json',
            data: {
                UserID: NameUser,
                GroupName: '',
                Tipe: filedown,
                Status: '',
                startDate: '',
                endDate: ''
            },
            success: function (responseData) {
                console.log(responseData);
                event.preventDefault();

                var base64String = responseData[0].File_pdf;
                var byteCharacters = atob(base64String);
                var byteNumbers = new Array(byteCharacters.length);
                for (var i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                var byteArray = new Uint8Array(byteNumbers);
                var blob = new Blob([byteArray], { type: "application/pdf" });
                var url = URL.createObjectURL(blob);
                var a = document.createElement("a");
                a.href = url;
                a.download = Namefile + ".pdf";
                document.body.appendChild(a);
                a.click();
                setTimeout(function () {
                    document.body.removeChild(a);
                    window.URL.revokeObjectURL(url);
                }, 0);
                $.unblockUI();
            },
            error: function (xhr, status, error) {
                console.error('Gagal mengambil data:', error);
                Swal.fire({
                    title: 'Gagal!',
                    text: 'Gagal Donwload File PDF Hasil cek ' + Namefile + ' Silahkan coba kembali beberapa saat lagi',
                    icon: 'warning',
                    showDenyButton: false,
                    showCancelButton: false,
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#6f42c1',
                });
                $.unblockUI();
            }
        });



    });
    $('#filter-btn').on('click', function () {
        $.blockUI({
            message: '<div class="spinner-border text-primary" role="status"></div>',
            css: {
                backgroundColor: 'transparent',
                border: '0'
            },
            overlayCSS: {
                backgroundColor: '#fff',
                opacity: 0.8
            }
        });
        var startDate = $('#start-date').val();
        var endDate = $('#end-date').val();

        // Validasi input tanggal
        if (!startDate || !endDate) {
            alert('Harap pilih rentang tanggal lengkap');
            return;
        }

        if (startDate > endDate) {
            alert('Tanggal mulai tidak boleh lebih besar dari tanggal akhir');
            return;
        }

        // Terapkan filter dan gambar ulang tabel
        dt_invoice.draw();
        $.unblockUI();
    });
    $('#reset-btn').on('click', function () {
        $.blockUI({
            message: '<div class="spinner-border text-primary" role="status"></div>',
            css: {
                backgroundColor: 'transparent',
                border: '0'
            },
            overlayCSS: {
                backgroundColor: '#fff',
                opacity: 0.8
            }
        });
        // Reset input tanggal
        $('#start-date, #end-date').val('');

        // Hapus filter dan tampilkan semua data
        dt_invoice.search('').columns().search('').draw();
        $.unblockUI();
    });


    function formatRupiah(angka) {
        // Membuat format angka menggunakan Intl.NumberFormat untuk format Indonesia
        var formattedNumber = new Intl.NumberFormat('id-ID', {
            style: 'currency',
            currency: 'IDR'
        }).format(angka);

        // Menghapus simbol "Rp" yang otomatis ditambahkan oleh Intl.NumberFormat
        formattedNumber = formattedNumber.replace('Rp', '').trim();

        // Menambahkan "Rp." di depan dan mengubah format untuk ribuan dan desimal
        formattedNumber = "Rp. " + formattedNumber.replace(',', '.') + ",-";

        return formattedNumber;
    }
    function getDateRange(dataArray) {
        // Pastikan dates adalah array dari tanggal yang valid
        if (!Array.isArray(dataArray)) {
            return null;
        }
        const validDates = dataArray
            .map(item => item.TglPengajuan) // Ambil nilai TglPengajuan
            .filter(date => date && !isNaN(new Date(date))); // Validasi tanggal
        console.log(validDates)
        if (validDates.length === 0) {
            console.log('tidak valid');
            return null;
        }

        // Konversi semua tanggal ke objek Date
        const parsedDates = validDates.map(date => new Date(date));

        // Cari tanggal paling awal dan paling akhir
        const earliestDate = new Date(Math.min.apply(null, parsedDates));
        const latestDate = new Date(Math.max.apply(null, parsedDates));

        return {
            startDate: earliestDate.toISOString().split('T')[0],
            endDate: latestDate.toISOString().split('T')[0]
        };
    }
    function terbilangRupiah(angka) {
        // Pastikan input adalah angka
        angka = Math.abs(Number(angka));

        // Daftar kata untuk angka
        const satuan = [
            '', 'Satu', 'Dua', 'Tiga', 'Empat', 'Lima',
            'Enam', 'Tujuh', 'Delapan', 'Sembilan', 'Sepuluh',
            'Sebelas', 'Dua Belas', 'Tiga Belas', 'Empat Belas',
            'Lima Belas', 'Enam Belas', 'Tujuh Belas', 'Delapan Belas', 'Sembilan Belas'
        ];

        const puluhan = [
            '', '', 'Dua Puluh', 'Tiga Puluh', 'Empat Puluh', 'Lima Puluh',
            'Enam Puluh', 'Tujuh Puluh', 'Delapan Puluh', 'Sembilan Puluh'
        ];

        // Fungsi rekursif untuk mengkonversi bilangan
        function convertToWords(num) {
            // Basis rekursi
            if (num < 20) return satuan[num];

            if (num < 100) {
                const depan = Math.floor(num / 10);
                const belakang = num % 10;
                return (belakang !== 0)
                    ? puluhan[depan] + ' ' + satuan[belakang]
                    : puluhan[depan];
            }

            if (num < 200) {
                return 'Seratus ' + convertToWords(num - 100);
            }

            if (num < 1000) {
                const depan = Math.floor(num / 100);
                const belakang = num % 100;
                return satuan[depan] + ' Ratus ' +
                    (belakang > 0 ? convertToWords(belakang) : '');
            }

            if (num < 2000) {
                return 'Seribu ' + convertToWords(num - 1000);
            }

            if (num < 1000000) {
                const depan = Math.floor(num / 1000);
                const belakang = num % 1000;
                return convertToWords(depan) + ' Ribu ' +
                    (belakang > 0 ? convertToWords(belakang) : '');
            }

            if (num < 1000000000) {
                const depan = Math.floor(num / 1000000);
                const belakang = num % 1000000;
                return convertToWords(depan) + ' Juta ' +
                    (belakang > 0 ? convertToWords(belakang) : '');
            }

            if (num < 1000000000000) {
                const depan = Math.floor(num / 1000000000);
                const belakang = num % 1000000000;
                return convertToWords(depan) + ' Miliar ' +
                    (belakang > 0 ? convertToWords(belakang) : '');
            }

            return 'Angka terlalu besar';
        }

        // Tangani kasus khusus
        if (angka === 0) return 'Nol Rupiah';

        // Konversi angka ke terbilang
        const terbilang = convertToWords(Math.floor(angka));

        // Tangani sen (2 digit di belakang koma)
        const sen = Math.round((angka - Math.floor(angka)) * 100);

        // Gabungkan terbilang dengan rupiah dan sen
        let hasil = terbilang + ' Rupiah';
        if (sen > 0) {
            hasil += ' ' + convertToWords(sen) + ' Sen';
        }

        return hasil;
    }


    //On each datatable draw, initialize tooltip
    dt_invoice_table.on('draw.dt', function () {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl, {
                boundary: document.body
            });
        });
    });

    // Delete Record
    $('.invoice-list-table tbody').on('click', '.delete-record', function () {
        // To hide tooltip on clicking delete icon
        $(this).closest($('[data-bs-toggle="tooltip"]').tooltip('hide'));
        // To delete the whole row
        dt_invoice.row($(this).parents('tr')).remove().draw();
    });

});
