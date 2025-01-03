$(document).ready(function () {
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
    function formatTanggal(tanggal) {
        // Array nama bulan dalam bahasa Indonesia
        const namaBulan = [
            'Januari', 'Februari', 'Maret', 'April', 'Mei', 'Juni',
            'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'
        ];

        // Buat objek Date dari string tanggal
        const date = new Date(tanggal);

        // Ekstrak hari, bulan, dan tahun
        const hari = date.getDate();
        const bulan = namaBulan[date.getMonth()];
        const tahun = date.getFullYear();

        // Kembalikan dalam format "DD Bulan YYYY"
        return `${hari} ${bulan} ${tahun}`;
    }
    function displayData() {
        // Retrieve data from localStorage
        var data = JSON.parse(localStorage.getItem("DataInv"));
        var dataNilai = JSON.parse(localStorage.getItem("totalPerhitungan"));
        if (dataNilai) {
            console.log(dataNilai);
            console.log(dataNilai.DataTerakhir);
            $("#jumlahpndb").empty();
            $("#jumlahjasa").empty();
            $("#pnbpnilai").empty();
            $("#periodenilai").empty();
            $("#dppnilai").empty();
            $("#ppnnilai").empty();
            $("#pphnilai").empty();
            $("#totalnilai").empty();
            $("#nilaiterbilang").empty();
            $("#jumlahpndb").text(dataNilai.jumlahcek_terakhir);
            $("#jumlahjasa").text(dataNilai.jumlahjasa);
            $("#pnbpnilai").text(dataNilai.DataTerakhir);
            $("#periodenilai").text(dataNilai.nilaiJasa);
            $("#dppnilai").text(dataNilai.nilaidpp);
            $("#ppnnilai").text(dataNilai.nilaippn);
            $("#pphnilai").text(dataNilai.nilaipph);
            $("#totalnilai").text(dataNilai.totalnilai);
            $("#nilaiterbilang").text(dataNilai.terbilang);
            $("#periodeakhir").text(formatTanggal(dataNilai.rentang.endDate));
            $("#periodemulai").text(formatTanggal(dataNilai.rentang.startDate));
            $("#tanggalbayar").text(formatTanggal(dataNilai.jatuhtempo));
            $("#tanggalmulai").text(formatTanggal(dataNilai.terbitinvo));
            $("#invociceno").text(dataNilai.nomorinv);
            $("#tanggalinvoicettd").text(formatTanggal(dataNilai.terbitinvo));
            $("#NamaCabang").text(dataNilai.Cabang);
            $("#NoFaktur").text(dataNilai.NoFaktur);
            const currentDate = new Date();
            const formattedDate = `${String(currentDate.getMonth() + 1).padStart(2, '0')}-${currentDate.getFullYear()}`;
            $("#BulanTerbit").text(formattedDate);

        }

        if (data && Array.isArray(data)) {
            $('#data-invoice tbody').empty();
            index = 0;
            $.each(data, function (index, item) {
                index++
                var row = $('<tr></tr>');
                row.append('<td hidden>' + item.NoTransaksi + '</td>');
                row.append('<td>' + index + '</td>');
                row.append('<td>' + item.TglPengajuan + '</td>');
                row.append('<td>' + item.NamaPT + '</td>');
                row.append('<td>' + item.Cabang + '</td>');
                if (item.TipeTransaksi === 'Data Terakhir') {
                    row.append('<td>' + item.Pnbpval + '</td>');
                } else {
                    row.append('<td>' + item.Pnbpval + '</td>');
                }
                row.append('<td>' + 'Rp. 20.000' + '</td>');
                $('#data-invoice tbody').append(row);
            });
            $.unblockUI();
        } else {
            console.log("No data found in localStorage.");
            $.unblockUI();
        }
    }
    $('#generateWord').click(function () {
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
        var tableData = [];
        $('#data-invoice tbody tr').each(function () {
            var row = [];
            $(this).find('td').each(function () {
                row.push($(this).text());
            });
            tableData.push(row);
        });
        var dataNilai = JSON.parse(localStorage.getItem("totalPerhitungan"));
        var startdate = dataNilai.rentang.startDate;
        var enddate = dataNilai.rentang.endDate;
        var cabangtxt = dataNilai.cabang;
        console.log(startdate + ' ' + enddate + ' ' + cabangtxt );
        // Ambil nilai div
        var invociceno = $("#invociceno").text();
        var tanggalmulai = $("#tanggalmulai").text();
        var tanggalbayar = $("#tanggalbayar").text();
        var pnbptext = $("#pnbptext").text();
        var jasatext = $("#jasatext").text();
        var periodedata = $("#periodedata").text();
        var pnbpnilai = $("#pnbpnilai").text();
        var periodenilai = $("#periodenilai").text();
        var dppnilai = $("#dppnilai").text();
        var ppnnilai = $("#ppnnilai").text();
        var pphnilai = $("#pphnilai").text();
        var totalnilai = $("#totalnilai").text();
        var nilaiterbilang = $("#nilaiterbilang").text();
        var tanggalinvoicettd = $("#tanggalinvoicettd").text();
        var dataToSend = {
            //tableData: tableData,
            invoice: invociceno,
            tglinv: tanggalmulai,
            tgltmp: tanggalbayar,
            pnbp: pnbptext,
            jasatot: jasatext,
            periodetot: periodedata,
            jpsatu: pnbpnilai,
            jidua: periodenilai,
            jdpp: dppnilai,
            jppn: ppnnilai,
            jpph: pphnilai,
            jtot: totalnilai,
            terbilang: nilaiterbilang,
            tanggalctts: tanggalinvoicettd,
            StartDate: startdate,
            EndDate: enddate,
            cabang: dataNilai.Cabang,
            NoFaktur: dataNilai.NoFaktur
        };
        function getByteSize(str) {
            return new Blob([str]).size;
        }

        var jsonString = JSON.stringify(dataToSend);
        var byteSize = getByteSize(jsonString);
        console.log("Ukuran dalam byte: " + byteSize);


        $.ajax({
            url: '/Home/GenerateWordTable',
            type: 'POST',
            timeout: 600000,
            data: JSON.stringify(dataToSend),
            contentType: 'application/json',
            xhrFields: {
                responseType: 'blob' // Menerima respons dalam bentuk blob
            },
            success: function (blob) {
                if (blob) {
                    // Buat URL dari blob file
                    var url = window.URL.createObjectURL(blob);

                    // Buat elemen <a> untuk memulai unduhan
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = 'INV_' + invociceno + '.docx'; // Nama file unduhan
                    document.body.appendChild(a);
                    a.click();

                    // Hapus elemen <a> setelah selesai
                    document.body.removeChild(a);
                    window.URL.revokeObjectURL(url);
                    $.unblockUI();
                }
                $.unblockUI();
            },
            error: function (err) {
                console.error('Terjadi kesalahan saat mengunduh file:', err);
                $.unblockUI();
                Swal.fire({
                    title: 'Gagal!',
                    text: 'Gagal Proses Gnerated Word',
                    icon: 'warning',
                    showDenyButton: false,
                    showCancelButton: false,
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#6f42c1',
                });
            }
        });
    });
    $('#SaveInvoice').click(function () {
        Swal.fire({
            title: 'Save Invoice',
            text: 'Anda yakin akan menyimpan invoice?',
            icon: 'warning',
            confirmButtonText: 'OK'
        }).then((result) => {
            if (result.isConfirmed) {
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
                var dataNilai = JSON.parse(localStorage.getItem("totalPerhitungan"));
                var startdate = dataNilai.rentang.startDate;
                var enddate = dataNilai.rentang.endDate;
                var cabangtxt = dataNilai.cabang;
                var invociceno = $("#invociceno").text();
                var tanggalmulai = $("#tanggalmulai").text();
                var tanggalbayar = $("#tanggalbayar").text();
                var pnbptext = $("#pnbptext").text();
                var jasatext = $("#jasatext").text();
                var periodedata = $("#periodedata").text();
                var pnbpnilai = $("#pnbpnilai").text();
                var periodenilai = $("#periodenilai").text();
                var dppnilai = $("#dppnilai").text();
                var ppnnilai = $("#ppnnilai").text();
                var pphnilai = $("#pphnilai").text();
                var totalnilai = $("#totalnilai").text();
                var nilaiterbilang = $("#nilaiterbilang").text();
                var tanggalinvoicettd = $("#tanggalinvoicettd").text();
                var dataToSend = {
                    invoice: invociceno,
                    tglinv: tanggalmulai,
                    tgltmp: tanggalbayar,
                    pnbp: pnbptext,
                    jasatot: jasatext,
                    periodetot: periodedata,
                    jpsatu: pnbpnilai,
                    jidua: periodenilai,
                    jdpp: dppnilai,
                    jppn: ppnnilai,
                    jpph: pphnilai,
                    jtot: totalnilai,
                    terbilang: nilaiterbilang,
                    tanggalctts: tanggalinvoicettd,
                    StartDate: startdate,
                    EndDate: enddate,
                    cabang: cabangtxt,
                    NoFaktur: dataNilai.NoFaktur
                };
                $.ajax({
                    url: '/Home/SaveDBInvoiceData',
                    type: 'POST',
                    data: JSON.stringify(dataToSend),
                    contentType: 'application/json',
                    success: function (blob) {
                        $.unblockUI();
                        Swal.fire({
                            title: 'Berhasil!',
                            text: 'Data berhasil disimpan',
                            icon: 'success',
                            showDenyButton: false,
                            showCancelButton: false,
                            confirmButtonText: 'OK',
                            confirmButtonColor: '#6f42c1',
                        });
                    },
                    error: function (err) {
                        Swal.fire({
                            title: 'Gagal!',
                            text: 'Gagal menyimpan data',
                            icon: 'warning',
                            showDenyButton: false,
                            showCancelButton: false,
                            confirmButtonText: 'OK',
                            confirmButtonColor: '#6f42c1',
                        });
                        console.error('Terjadi kesalahan saat mengunduh file:', err);

                    }
                });
            }
        });
       
    });
    $('#generatePDF').click(function () {
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
        var tableData = [];
        $('#data-invoice tbody tr').each(function () {
            var row = [];
            $(this).find('td').each(function () {
                row.push($(this).text());
            });
            tableData.push(row);
        });
        var dataNilai = JSON.parse(localStorage.getItem("totalPerhitungan"));
        var startdate = dataNilai.rentang.startDate;
        var enddate = dataNilai.rentang.endDate;
        var cabangtxt = dataNilai.cabang;
        var invociceno = $("#invociceno").text();
        var tanggalmulai = $("#tanggalmulai").text();
        var tanggalbayar = $("#tanggalbayar").text();
        var pnbptext = $("#pnbptext").text();
        var jasatext = $("#jasatext").text();
        var periodedata = $("#periodedata").text();
        var pnbpnilai = $("#pnbpnilai").text();
        var periodenilai = $("#periodenilai").text();
        var dppnilai = $("#dppnilai").text();
        var ppnnilai = $("#ppnnilai").text();
        var pphnilai = $("#pphnilai").text();
        var totalnilai = $("#totalnilai").text();
        var nilaiterbilang = $("#nilaiterbilang").text();
        var tanggalinvoicettd = $("#tanggalinvoicettd").text();
        var dataToSend = {
            //tableData: tableData,
            invoice: invociceno,
            tglinv: tanggalmulai,
            tgltmp: tanggalbayar,
            pnbp: pnbptext,
            jasatot: jasatext,
            periodetot: periodedata,
            jpsatu: pnbpnilai,
            jidua: periodenilai,
            jdpp: dppnilai,
            jppn: ppnnilai,
            jpph: pphnilai,
            jtot: totalnilai,
            terbilang: nilaiterbilang,
            tanggalctts: tanggalinvoicettd,
            StartDate: startdate,
            EndDate: enddate,
            cabang: cabangtxt,
            NoFaktur: dataNilai.NoFaktur
        };
        function getByteSize(str) {
            return new Blob([str]).size;
        }

        var jsonString = JSON.stringify(dataToSend);
        var byteSize = getByteSize(jsonString);
        console.log("Ukuran dalam byte: " + byteSize);


        $.ajax({
            url: '/Home/GenerateFilePdf',
            type: 'POST',
            timeout: 600000,
            data: JSON.stringify(dataToSend),
            contentType: 'application/json',
            xhrFields: {
                responseType: 'blob' // Menerima respons dalam bentuk blob
            },
            success: function (blob) {
                if (blob) {
                    // Buat URL dari blob file
                    var url = window.URL.createObjectURL(blob);

                    // Buat elemen <a> untuk memulai unduhan
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = 'INV_' + invociceno + '.pdf'; // Nama file unduhan
                    document.body.appendChild(a);
                    a.click();

                    // Hapus elemen <a> setelah selesai
                    document.body.removeChild(a);
                    window.URL.revokeObjectURL(url);
                    $.unblockUI();
                }
                $.unblockUI();
            },
            error: function (err) {
                console.error('Terjadi kesalahan saat mengunduh file:', err);
                $.unblockUI();
                Swal.fire({
                    title: 'Gagal!',
                    text: 'Gagal Proses Gnerated PDF',
                    icon: 'warning',
                    showDenyButton: false,
                    showCancelButton: false,
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#6f42c1',
                });
            }
        });
    })
    displayData();
});




