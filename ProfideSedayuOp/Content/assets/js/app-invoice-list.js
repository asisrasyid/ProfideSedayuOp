/**
 * App Invoice List (jquery)
 */

'use strict';

$(function () {
    // Variable declaration for table
    var dt_invoice_table = $('.invoice-list-table');

    // Invoice datatable
    if (dt_invoice_table.length) {
        var dt_invoice = dt_invoice_table.DataTable({
            ajax: //assetsPath + 'json/invoice-list.json', // JSON file to add data
            {
                url: '/Home/GetDataPengecekan', // URL endpoint yang digunakan
                type: 'POST', // Menggunakan metode POST untuk mengirim parameter
                data: function (d) {
                    // Menambahkan parameter ke permintaan AJAX
                    d.User_Pengaju = '';
                    d.User_Group = '';
                    d.tipe = '002';
                    d.status = '';
                    d.startDate = '';
                    d.endDate = '';
                },
                dataSrc: function (json) {
                    console.log('Data JSON:', json); // Tampilkan data
                    return json;
                }
            },
            columns: [
                // columns according to JSON
                { data: 'No_Invoice' },
                { data: 'No_Invoice' },
                { data: 'No_Invoice' },
                { data: 'IsPaid' },
                { data: 'Tgl_JatuhTempo' },
                { data: 'Client' },
                { data: 'TotalNilai' },
                { data: 'CreateBy' },
                { data: 'IsPaid' },
                { data: 'action' }
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
                    // For Checkboxes
                    targets: 1,
                    orderable: false,
                    checkboxes: {
                        selectAllRender: '<input type="checkbox" class="form-check-input">'
                    },
                    render: function () {
                        return '<input type="checkbox" class="dt-checkboxes form-check-input">';
                    },
                    searchable: false
                },
                {
                    // Invoice ID
                    targets: 2,
                    render: function (data, type, full, meta) {
                        var $invoice_id = full['No_Invoice'];
                        // Creates full output for row
                        var $row_output = '<a href=""><span>' + $invoice_id + '</span></a>';
                        return $row_output;
                    }
                },
                {
                    // Invoice status
                    targets: 3,
                    render: function (data, type, full, meta) {
                        var $invoice_status = full['IsPaid'],
                            $due_date = full['Tgl_JatuhTempo'],
                            $balance = full['TotalNilai'];
                        var roleBadgeObj = {
                            Sent: '<span class="avatar avatar-sm"> <span class="avatar-initial rounded-circle bg-label-secondary"><i class="ri-save-line ri-16px"></i></span></span>',
                            Draft:
                                '<span class="avatar avatar-sm"> <span class="avatar-initial rounded-circle bg-label-primary"><i class="ri-mail-line ri-16px"></i></span></span>',
                            'Past Due':
                                '<span class="avatar avatar-sm"> <span class="avatar-initial rounded-circle bg-label-danger"><i class="ri-error-warning-line ri-16px"></i></span></span>',
                            'Partial Payment':
                                '<span class="avatar avatar-sm"> <span class="avatar-initial rounded-circle bg-label-success"><i class="ri-check-line ri-16px"></i></span></span>',
                            Paid: '<span class="avatar avatar-sm"> <span class="avatar-initial rounded-circle bg-label-warning"><i class="ri-line-chart-line ri-16px"></i></span></span>',
                            Downloaded:
                                '<span class="avatar avatar-sm"> <span class="avatar-initial rounded-circle bg-label-info"><i class="ri-arrow-down-line ri-16px"></i></span></span>'
                        };
                        return (
                            "<div class='d-inline-flex' data-bs-toggle='tooltip' data-bs-html='true' title='<span>" +
                            $invoice_status +
                            '<br> <span class="fw-medium">Balance:</span> ' +
                            $balance +
                            '<br> <span class="fw-medium">Due Date:</span> ' +
                            $due_date +
                            "</span>'>" +
                            $invoice_status +
                            '</div>'
                        );
                    }
                },
                {
                    // Client name and email
                    targets: 4,
                    responsivePriority: 4,
                    render: function (data, type, full, meta) {
                        var $name = full['Client'],
                            $email = full['Client'],
                            $image = full['Client'],
                            $rand_num = Math.floor(Math.random() * 11) + 1,
                            $user_img = $rand_num + '.png';
                        if ($image === "true") {
                            // For Avatar image
                            var $output =
                                '<img src="' + assetsPath + 'img/avatars/' + $user_img + '" alt="Avatar" class="rounded-circle">';
                        } else {
                            // For Avatar badge
                            var stateNum = Math.floor(Math.random() * 6),
                                states = ['success', 'danger', 'warning', 'info', 'dark', 'primary', 'secondary'],
                                $state = states[stateNum],
                                $name = full['Client'],
                                $initials = $name.match(/\b\w/g) || [];
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
                    // Total Invoice Amount
                    targets: 5,
                    render: function (data, type, full, meta) {
                        var $total = full['TotalNilai'];
                        return '<span class="d-none">' + $total + '</span>' + $total;
                    }
                },
                {
                    // Due Date
                    targets: 6,
                    render: function (data, type, full, meta) {
                        var $due_date = full['Tgl_JatuhTempo'];
                        // Creates full output for row
                        var $row_output =
                            '<span class="d-none">' +
                            ($due_date)+
                            '</span>' +
                            ($due_date);
                        $due_date;
                        return $row_output;
                    }
                },
                {
                    // Client Balance/Status
                    targets: 7,
                    orderable: true,
                    render: function (data, type, full, meta) {
                        var $balance = full['CreateBy'];
                        if ($balance) {
                            return '<span class="text-heading">' + $balance + '</span>';
                        } else {
                            return '<span class="text-heading">' + $balance + '</span>';
                        }
                    }
                },
                {
                    targets: 8,
                    visible: false
                },
                {
                    // Actions
                    targets: -1,
                    title: 'Actions',
                    searchable: false,
                    orderable: false,
                    render: function (data, type, full, meta) {
                        var $datadown = full['File'];
                        var $noinv = full['No_Invoice'];
                        return (
                            `<div class="d-flex align-items-center">` +
                            `<button data-bs-toggle="tooltip" class="btn btn-sm btn-icon btn-te+xt-secondary rounded-pill waves-effect waves-light delete-record" data-bs-placement="top" data-file="${$datadown}"title="Download Invoice"><i class="ri-folder-download-line ri-22px"></i></button>` +
                            `<button data-bs-toggle="tooltip" class="btn btn-sm btn-icon btn-text-secondary rounded-pill waves-effect waves-light update-record" data-bs-placement="top" data-no="${$noinv}" title="Update Status Invoice"><i class="ri-loop-right-line ri-22px"></i></button>`
                           
                        );
                    }
                }
            ],
            order: [[2, 'desc']],
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
                searchPlaceholder: 'Search Invoice'
            },
            // Buttons with Dropdown
            buttons: [
                {
                    text: '<i class="ri-add-line ri-16px me-md-1_5"></i><span class="d-md-inline-block d-none">Create Invoice</span>',
                    className: 'btn btn-primary waves-effect waves-light',
                    action: function (e, dt, button, config) {
                        window.location = 'app_invoice_select';
                    }
                }
            ],
            // For responsive popup
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.modal({
                        header: function (row) {
                            var data = row.data();
                            return 'Details of ' + data['Client'];
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
                            '<select id="UserRole" class="form-select form-select-sm"><option value=""> Invoice Status </option></select>'
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
    }

    // On each datatable draw, initialize tooltip
    dt_invoice_table.on('draw.dt', function () {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl, {
                boundary: document.body
            });
        });
    }); 
    $('.invoice-list-table tbody').on('click', '.update-record', function () {
        var noinv = $(this).data("no");
        $.ajax({
            url: '/Home/SaveDataPengecekan', // Ganti dengan endpoint server Anda
            method: 'POST',
            dataType: 'json',
            data: {
                NoTransksi: noinv,
                Status: '101'
            },
            success: function (responseData) {
                console.log(responseData);
                Swal.fire({
                    title: 'Berhasil!',
                    text: 'Invoice' + noinv + ' Berhasil di Update',
                    icon: 'Success',
                    showDenyButton: false,
                    showCancelButton: false,
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#6f42c1',
                });
                $.unblockUI();
            },
            error: function (xhr, status, error) {
               
                console.error('Gagal mengambil data:', error);
                Swal.fire({
                    title: 'Gagal!',
                    text: 'Gagal Proses Gnerated PDF',
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
    // Download data 
    $('.invoice-list-table tbody').on('click', '.delete-record', function () {
        var filedown = $(this).data("file");
        $.ajax({
            url: '/Home/GetDataPengecekan', // Ganti dengan endpoint server Anda
            method: 'POST',
            dataType: 'json',
            data: {
                UserID: '',
                GroupName: '',
                Tipe: filedown,
                Status: '',
                startDate: '',
                endDate: ''
            },
            success: function (responseData) {
                console.log(responseData);
                event.preventDefault();

                var base64String = responseData[0].File;
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
                a.download = filedown + ".pdf";
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
                    text: 'Gagal Proses Download PDF',
                    icon: 'warning',
                    showDenyButton: false,
                    showCancelButton: false,
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#6f42c1',
                }); $.unblockUI();
            }
        });


        // To hide tooltip on clicking delete icon
        
        // To delete the whole row
        
    });
});
