/**
 * Dashboards Analytics
 */

'use strict';

(function () {
    let cardColor, labelColor, borderColor, chartBgColor, bodyColor, legendColor, isDarkStyle;

    if (isDarkStyle) {
        cardColor = config.colors_dark.cardColor;
        labelColor = config.colors_dark.textMuted;
        borderColor = config.colors_dark.borderColor;
        chartBgColor = config.colors_dark.chartBgColor;
        bodyColor = config.colors_dark.bodyColor;
        legendColor = config.colors_dark.bodyColor;
    } else {
        cardColor = config.colors.cardColor;
        labelColor = config.colors.textMuted;
        borderColor = config.colors.borderColor;
        chartBgColor = config.colors.chartBgColor;
        bodyColor = config.colors.bodyColor;
        legendColor = config.colors.bodyColor;
    }
    const chartColors = {
        column: {
            series1: '#826af9',
            series2: '#d2b0ff',
            bg: '#f8d3ff',
            series3: '#FFB200'
        },
        donut: {
            series1: '#fdd835',
            series2: '#32baff',
            series3: '#ffa1a1',
            series4: '#7367f0',
            series5: '#29dac7'
        },
        area: {
            series1: '#ab7efd',
            series2: '#b992fe',
            series3: '#e0cffe'
        }
    };

    // Weekly Overview Line Chart
    // --------------------------------------------------------------------
    const weeklyOverviewChartEl = document.querySelector('#weeklyOverviewChart');
    const monthlyOverviewChartEl = document.querySelector('#monthlyOverviewChart');
    const performanceChartEl = document.querySelector('#performanceChart');
    const performanceChartMit = document.querySelector('#performanceChartMit');
    function countDataPerDay(data) {
        const today = new Date();
        const oneWeekAgo = new Date(today);
        oneWeekAgo.setDate(today.getDate() - 7); // Set tanggal ke 1 minggu sebelumnya

        const dailyCounts = {};
        const dailyLabels = [];//['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

        // Inisialisasi jumlah data untuk setiap hari
        for (let i = 0; i <= 7; i++) {
            const date = new Date(oneWeekAgo);
            date.setDate(oneWeekAgo.getDate() + i);
            const formattedDate = date.toISOString().split('T')[0];
            dailyCounts[formattedDate] = 0;

            // Format label untuk tampilan (misalnya DD/MM)
            const labelFormat = date.toLocaleDateString('id-ID', {
                day: '2-digit',
                month: '2-digit'
            });
            dailyLabels.push(labelFormat);
        }

        // Hitung jumlah data berdasarkan tanggal_pengajuan
        data.forEach(item => {
            const itemDate = new Date(item.Tgl_Update).toISOString().split('T')[0];
            if (dailyCounts[itemDate] !== undefined) {
                dailyCounts[itemDate]++;
            }
        });

        const countsArray = Object.keys(dailyCounts).map(date => dailyCounts[date]);

        return {
            counts: countsArray,
            labels: dailyLabels
        };
    }
    function countDataPerMonth(data) {
        // Objek untuk memetakan angka bulan ke nama bulan dalam Indonesia
        const monthNames = {
            0: 'Januari', 1: 'Februari', 2: 'Maret', 3: 'April',
            4: 'Mei', 5: 'Juni', 6: 'Juli', 7: 'Agustus',
            8: 'September', 9: 'Oktober', 10: 'November', 11: 'Desember'
        };

        // Objek untuk menyimpan jumlah data per bulan
        const monthlyCounts = {};
        const monthlyLabels = [];

        const currentDate = new Date();
        const currentYear = currentDate.getFullYear();
        const currentMonth = currentDate.getMonth();

        // Menghitung batas tanggal untuk 5 bulan terakhir
        const startDate = new Date(currentYear, currentMonth - 4, 1);

        // Filter data untuk hanya 5 bulan terakhir
        const filteredData = data.filter(item => {
            const itemDate = new Date(item.Tgl_Pengajuan);
            return itemDate >= startDate && itemDate <= currentDate;
        });

        // Cari rentang tahun dari data
        const years = [...new Set(filteredData.map(item => new Date(item.Tgl_Pengajuan).getFullYear()))];

        // Urutkan tahun
        years.sort();

        // Hitung data per bulan untuk setiap tahun
        years.forEach(year => {
            // Filter data untuk tahun tertentu
            const yearData = filteredData.filter(item => new Date(item.Tgl_Pengajuan).getFullYear() === year);

            // Inisialisasi hitungan untuk 12 bulan
            for (let month = 0; month < 12; month++) {
                const key = `${year}-${month}`;
                monthlyCounts[key] = 0;
            }

            // Hitung jumlah data per bulan
            yearData.forEach(item => {
                const itemDate = new Date(item.Tgl_Pengajuan);
                const key = `${itemDate.getFullYear()}-${itemDate.getMonth()}`;
                monthlyCounts[key]++;
            });
        });

        // Siapkan data untuk chart
       
        const countsArray = [];
        const labelsArray = [];

        // Urutkan kunci untuk memastikan urutan yang benar
        //const sortedKeys = Object.keys(monthlyCounts).sort();

        const sortedKeys = Object.keys(monthlyCounts).sort((a, b) => {
            const [yearA, monthA] = a.split('-').map(Number);
            const [yearB, monthB] = b.split('-').map(Number);

            // Bandingkan tahun, jika sama bandingkan bulan
            return yearA - yearB || monthA - monthB;
        });
        const lastFiveKeys = sortedKeys.slice(-5);
        lastFiveKeys.forEach(key => {
            const [year, month] = key.split('-').map(Number);
            countsArray.push(monthlyCounts[key]);
            labelsArray.push(`${monthNames[month]} ${year}`);
        });

        return {
            counts: countsArray,
            labels: labelsArray
        };
    }
    function countDataPerGropMont(data) {
        // Month names in Indonesian
        const monthNames = {
            0: 'Januari', 1: 'Februari', 2: 'Maret', 3: 'April',
            4: 'Mei', 5: 'Juni', 6: 'Juli', 7: 'Agustus',
            8: 'September', 9: 'Oktober', 10: 'November', 11: 'Desember'
        };

        const monthlyCounts = {};
        const currentDate = new Date();
        const currentYear = currentDate.getFullYear();
        const currentMonth = currentDate.getMonth();

        // Get data for last 5 months
        const startDate = new Date(currentYear, currentMonth - 4, 1);

        // Filter data for last 5 months
        const filteredData = data.filter(item => {
            const itemDate = new Date(item.Tgl_Pengajuan);
            return itemDate >= startDate && itemDate <= currentDate;
        });

        // Get unique years
        const years = [...new Set(filteredData.map(item => new Date(item.Tgl_Pengajuan).getFullYear()))];
        years.sort();

        // Calculate data per month for each year
        years.forEach(year => {
            const yearData = filteredData.filter(item =>
                new Date(item.Tgl_Pengajuan).getFullYear() === year
            );

            // Initialize counts for 12 months
            for (let month = 0; month < 12; month++) {
                const key = `${year}-${month}`;
                monthlyCounts[key] = {
                    'PT': 0,
                    'CV': 0,
                    'Lainnya': 0
                };
            }

            // Count data per month and PT type
            yearData.forEach(item => {
                const itemDate = new Date(item.Tgl_Pengajuan);
                const key = `${itemDate.getFullYear()}-${itemDate.getMonth()}`;

                // Determine PT type
                let ptType = 'Lainnya';
                if (item.Nama_PT) {
                    const namaPT = item.Nama_PT.toUpperCase();
                    if (item.Jenis_Badan === "cv" || namaPT.trim().slice(0, 2) === "cv") {
                        ptType = 'CV';
                    } else if (item.Jenis_Badan === "pt" || namaPT.trim().slice(0, 2) === "pt") {
                        ptType = 'PT';
                    }
                }

                // Increment counter
                monthlyCounts[key][ptType]++;
            });
        });

        // Prepare data for chart
        const resultData = {};
        const labelsArray = [];

        // Sort keys to ensure correct order
        const sortedKeys = Object.keys(monthlyCounts).sort((a, b) => {
            const [yearA, monthA] = a.split('-').map(Number);
            const [yearB, monthB] = b.split('-').map(Number);
            return yearA - yearB || monthA - monthB;
        });

        // Take last 5 keys
        const lastFiveKeys = sortedKeys.slice(-5);

        // Create result object
        lastFiveKeys.forEach(key => {
            const [year, month] = key.split('-').map(Number);
            const labelKey = `${monthNames[month]} ${year}`;

            resultData[labelKey] = monthlyCounts[key];
            labelsArray.push(labelKey);
        });

        return {
            labels: labelsArray,
            data: resultData
        };
    }

    // Initialize chart function
    function initializeChartJenisBadan(jenisBadanDataMitsui, jenisBadanDataMTF) {
        // Prepare data for ApexCharts
        const series = [
            {
                name: 'CV - Mitsui',
                data: jenisBadanDataMitsui.labels.map(label => jenisBadanDataMitsui.data[label]['CV'])
            },
            {
                name: 'PT - Mitsui',
                data: jenisBadanDataMitsui.labels.map(label => jenisBadanDataMitsui.data[label]['PT'])
            },
            {
                name: 'Lainnya - Mitsui',
                data: jenisBadanDataMitsui.labels.map(label => jenisBadanDataMitsui.data[label]['Lainnya'])
            },
            {
                name: 'CV - MTF',
                data: jenisBadanDataMTF.labels.map(label => jenisBadanDataMTF.data[label]['CV'])
            },
            {
                name: 'PT - MTF',
                data: jenisBadanDataMTF.labels.map(label => jenisBadanDataMTF.data[label]['PT'])
            },
            {
                name: 'Lainnya - MTF',
                data: jenisBadanDataMTF.labels.map(label => jenisBadanDataMTF.data[label]['Lainnya'])
            }
        ];

        const options = {
            chart: {
                type: 'bar',
                height: 400,
                stacked: true,
                toolbar: {
                    show: true
                }
            },
            series: series,
            xaxis: {
                categories: jenisBadanDataMitsui.labels,
                labels: {
                    style: {
                        fontSize: '12px',
                        fontFamily: 'Inter',
                        fontWeight: 400,
                        colors: '#6c757d'
                    }
                }
            },
            colors: ['#007bff', '#28a745', '#dc3545', '#17a2b8', '#ffc107', '#6c757d'],
            legend: {
                position: 'bottom',
                offsetY: 10
            },
            plotOptions: {
                bar: {
                    horizontal: false,
                    columnWidth: '60%'
                }
            }
        };

        const chartEl = document.querySelector('#performanceChart');
        if (chartEl) {
            const chart = new ApexCharts(chartEl, options);
            chart.render();
        }
    }

    function countDataPerGropMontOld(data) {
        // Objek untuk memetakan angka bulan ke nama bulan dalam Indonesia
        const monthNames = {
            0: 'Januari', 1: 'Februari', 2: 'Maret', 3: 'April',
            4: 'Mei', 5: 'Juni', 6: 'Juli', 7: 'Agustus',
            8: 'September', 9: 'Oktober', 10: 'November', 11: 'Desember'
        };

        // Objek untuk menyimpan jumlah data per bulan dan jenis PT
        const monthlyCounts = {};
        const currentDate = new Date();
        const currentYear = currentDate.getFullYear();
        const currentMonth = currentDate.getMonth();

        // Menghitung batas tanggal untuk 5 bulan terakhir
        const startDate = new Date(currentYear, currentMonth - 4, 1);

        // Filter data untuk hanya 5 bulan terakhir
        const filteredData = data.filter(item => {
            const itemDate = new Date(item.Tgl_Pengajuan);
            return itemDate >= startDate && itemDate <= currentDate;
        });

        // Cari rentang tahun dari data
        const years = [...new Set(filteredData.map(item => new Date(item.Tgl_Pengajuan).getFullYear()))];
        years.sort();

        // Hitung data per bulan untuk setiap tahun
        years.forEach(year => {
            // Filter data untuk tahun tertentu
            const yearData = filteredData.filter(item => new Date(item.Tgl_Pengajuan).getFullYear() === year);

            // Inisialisasi hitungan untuk 12 bulan
            for (let month = 0; month < 12; month++) {
                const key = `${year}-${month}`;
                monthlyCounts[key] = {
                    'PT': 0,
                    'CV': 0,
                    'Lainnya': 0
                };
            }

            // Hitung jumlah data per bulan dan jenis PT
            yearData.forEach(item => {
                const itemDate = new Date(item.Tgl_Pengajuan);
                const key = `${itemDate.getFullYear()}-${itemDate.getMonth()}`;

                // Tentukan jenis PT, default ke 'Lainnya' jika tidak sesuai
                let ptType = 'Lainnya';
                if (item.Nama_PT ) {
                    const namaPT = item.Nama_PT.toUpperCase();
                    if (item.Jenis_Badan === "cv" || namaPT.trim().slice(0, 2) === "cv") {
                        ptType = 'CV';
                    } else if (item.Jenis_Badan === "pt" || namaPT.trim().slice(0, 2) === "cv") {
                        ptType = 'PT';
                    } else if ((item.Jenis_Badan != "pt" || item.Nama_PT.trim().slice(0, 2).toLowerCase() != "pt") || (item.Jenis_Badan != "cv" || item.Nama_PT.trim().slice(0, 2).toLowerCase() != "cv")) {
                        ptType = 'Lainnya';
                    }
                }

                // Increment counter
                monthlyCounts[key][ptType]++;
            });
        });

        // Siapkan data untuk chart
        const resultData = {};
        const labelsArray = [];

        // Urutkan kunci untuk memastikan urutan yang benar
        const sortedKeys = Object.keys(monthlyCounts).sort((a, b) => {
            const [yearA, monthA] = a.split('-').map(Number);
            const [yearB, monthB] = b.split('-').map(Number);
            // Bandingkan tahun, jika sama bandingkan bulan
            return yearA - yearB || monthA - monthB;
        });

        // Ambil 5 kunci terakhir
        const lastFiveKeys = sortedKeys.slice(-5);

        // Buat objek hasil
        lastFiveKeys.forEach(key => {
            const [year, month] = key.split('-').map(Number);
            const labelKey = `${monthNames[month]} ${year}`;

            resultData[labelKey] = monthlyCounts[key];
            labelsArray.push(labelKey);
        });

        return {
            labels: labelsArray,
            data: resultData
        };
    }

    function countDataPerMonthJenisBadan(data) {
        // Objek untuk memetakan angka bulan ke nama bulan dalam Indonesia
        const monthNames = {
            0: 'Januari', 1: 'Februari', 2: 'Maret', 3: 'April',
            4: 'Mei', 5: 'Juni', 6: 'Juli', 7: 'Agustus',
            8: 'September', 9: 'Oktober', 10: 'November', 11: 'Desember'
        };

        // Objek untuk menyimpan jumlah data per bulan untuk setiap kategori
        const monthlyCountsCV = {};
        const monthlyCountsPT = {};
        const monthlyCountsElse = {};


        const currentDate = new Date();
        const currentYear = currentDate.getFullYear();
        const currentMonth = currentDate.getMonth();

        // Menghitung batas tanggal untuk 5 bulan terakhir
        const startDate = new Date(currentYear, currentMonth - 4, 1);

        // Filter data untuk hanya 5 bulan terakhir
        const filteredData = data.filter(item => {
            const itemDate = new Date(item.Tgl_Pengajuan);
            return itemDate >= startDate && itemDate <= currentDate;
        });


        // Cari rentang tahun dari data
        const years = [...new Set(filteredData.map(item => new Date(item.Tgl_Pengajuan).getFullYear()))];

        // Urutkan tahun
        years.sort();

        // Hitung data per bulan untuk setiap tahun dan kategori
        years.forEach(year => {
            // Filter data untuk tahun tertentu
            const yearData = filteredData.filter(item => new Date(item.Tgl_Pengajuan).getFullYear() === year);

            // Inisialisasi hitungan untuk 12 bulan pada setiap kategori
            for (let month = 0; month < 12; month++) {
                const key = `${year}-${month}`;
                monthlyCountsCV[key] = 0;
                monthlyCountsPT[key] = 0;
                monthlyCountsElse[key] = 0;
            }

            // Hitung jumlah data per bulan berdasarkan kategori
            yearData.forEach(item => {
                const itemDate = new Date(item.Tgl_Pengajuan);
                const key = `${itemDate.getFullYear()}-${itemDate.getMonth()}`;

                if (item.Jenis_Badan === "cv" || item.Nama_PT.trim().slice(0, 2).toLowerCase() === "cv") {
                    monthlyCountsCV[key]++;
                } else if (item.Jenis_Badan === "pt" || item.Nama_PT.trim().slice(0, 2).toLowerCase() === "pt") {
                    monthlyCountsPT[key]++;
                } else if ((item.Jenis_Badan != "pt" || item.Nama_PT.trim().slice(0, 2).toLowerCase() != "pt") || (item.Jenis_Badan != "cv" || item.Nama_PT.trim().slice(0, 2).toLowerCase() != "cv")) {
                    monthlyCountsElse[key]++;
                }
            });
        });

        // Siapkan data untuk chart
        const countsArrayCV = [];
        const countsArrayPT = [];
        const countsArrayElse = [];
        const labelsArray = [];

        // Urutkan kunci untuk memastikan urutan yang benar
        const sortedKeys = Object.keys(monthlyCountsCV).sort((a, b) => {
            const [yearA, monthA] = a.split('-').map(Number);
            const [yearB, monthB] = b.split('-').map(Number);

            // Bandingkan tahun, jika sama bandingkan bulan
            return yearA - yearB || monthA - monthB;
        });


        sortedKeys.forEach(key => {
            const [year, month] = key.split('-').map(Number);

            // Hanya tambahkan bulan yang memiliki data untuk setidaknya satu kategori
            if (monthlyCountsCV[key] > 0 || monthlyCountsPT[key] > 0 || monthlyCountsElse[key] > 0) {
                countsArrayCV.push(monthlyCountsCV[key]);
                countsArrayPT.push(monthlyCountsPT[key]);
                countsArrayElse.push(monthlyCountsElse[key]);
                labelsArray.push(`${monthNames[month]} ${year}`);
            }
        });

        return {
            countsCV: countsArrayCV,
            countsPT: countsArrayPT,
            countsElse: countsArrayElse,
            labels: labelsArray
        };
    }


    function initializeChartMonth(monthlyDataMitsui, monthlyDataMandiri) {
        // Gabungkan label dari kedua dataset untuk membuat kategori unik
        const combinedLabels = [...new Set([...monthlyDataMitsui.labels, ...monthlyDataMandiri.labels])];

        // Fungsi untuk menyelaraskan data berdasarkan label gabungan
        function alignData(labels, dataset) {
            const dataMap = Object.fromEntries(dataset.labels.map((label, index) => [label, dataset.counts[index]]));
            return labels.map(label => dataMap[label] || 0); // Isi dengan 0 jika label tidak ditemukan
        }

        // Selaraskan data Mitsui dan Mandiri
        const alignedMitsuiData = alignData(combinedLabels, monthlyDataMitsui);
        const alignedMandiriData = alignData(combinedLabels, monthlyDataMandiri);

        const monthlyOverviewChartConfig = {
            chart: {
                type: 'bar',
                height: 300,
                offsetY: -9,
                offsetX: -16,
                parentHeightOffset: 0,
                toolbar: {
                    show: false
                }
            },
            series: [
                {
                    name: 'Jumlah Data Mitsui',
                    data: alignedMitsuiData
                },
                {
                    name: 'Jumlah Data MTF',
                    data: alignedMandiriData
                }
            ],
            colors: ['#9694FF', '#6c48c5'],
            plotOptions: {
                bar: {
                    borderRadius: 6,
                    columnWidth: '80%',
                    endingShape: 'rounded',
                    startingShape: 'rounded'
                }
            },
            dataLabels: {
                enabled: true,
                formatter: function (val) {
                    return val > 0 ? val : '';
                },
                style: {
                    colors: ['#000'],
                    fontSize: '18px'
                }
            },
            legend: {
                show: true, // Menampilkan legend
                position: 'top', // Posisi legend ('top', 'bottom', 'left', 'right')
                horizontalAlign: 'center', // Penyusunan legend ('left', 'center', 'right')
                fontSize: '14px',
                markers: {
                    width: 12,
                    height: 12,
                    radius: 12 // Membuat marker berbentuk lingkaran
                },
                itemMargin: {
                    horizontal: 10,
                    vertical: 5
                }
            },
            grid: {
                strokeDashArray: 8,
                borderColor,
                padding: {
                    bottom: -10
                }
            },
            xaxis: {
                categories: combinedLabels,
                axisTicks: { show: false },
                crosshairs: { opacity: 0 },
                axisBorder: { show: false },
                tickPlacement: 'on',
                labels: {
                    show: true,
                    //rotate: -45,
                    //rotateAlways: true,
                    style: {
                        fontSize: '16px',
                        fontWeight: '4'
                    }
                }
            },
            yaxis: {
                min: 0,
                max: Math.max(...alignedMitsuiData.concat(alignedMandiriData)) * 1.2, // Dinamis sesuai data
                show: true,
                tickAmount: 3,
                labels: {
                    formatter: function (val) {
                        return parseInt(val);
                    },
                    style: {
                        fontSize: '14px',
                        fontFamily: 'Inter',
                        colors: labelColor
                    }
                }
            },
            states: {
                hover: {
                    filter: {
                        type: 'none'
                    }
                },
                active: {
                    filter: {
                        type: 'none'
                    }
                }
            },
            responsive: [
                {
                    breakpoint: 1500,
                    options: {
                        plotOptions: {
                            bar: {
                                columnWidth: '40%'
                            }
                        }
                    }
                }
            ]
        };

        if (monthlyOverviewChartEl) {
            const monthlyOverviewChart = new ApexCharts(monthlyOverviewChartEl, monthlyOverviewChartConfig);
            monthlyOverviewChart.render();
        }
    }
    function initializeChart(dailyDataMit, dailyDataMtf) {
        const weeklyOverviewChartConfig = {
            chart: {
                type: 'bar',
                height: 300,
                offsetY: -9,
                offsetX: -16,
                parentHeightOffset: 0,
                toolbar: {
                    show: false
                }
            },
            series: [
                {
                    name: 'Jumlah Data Mitsui',
                    data: dailyDataMit.counts,
                    color: '#9694FF' 
                },
                {
                    name: 'Jumlah Data MTF',
                    data: dailyDataMtf.counts,
                    color: '#6c48c5'
                }
            ],
            colors: ['#9694FF', '#6c48c5'],
            plotOptions: {
                bar: {
                    borderRadius: 6,
                    columnWidth: '80%',
                    endingShape: 'rounded',
                    startingShape: 'rounded'
                    //colors: {
                    //    ranges: [
                    //        {
                    //            from: 75,
                    //            to: 80,
                    //            color: config.colors.primary
                    //        },
                    //        {
                    //            from: 0,
                    //            to: 73,
                    //            color: config.colors.primary
                    //        }
                    //    ]
                    //}
                }
            },
            dataLabels: {
                enabled: true,
                formatter: function (val) {
                    return val > 0 ? val : '';
                },
                style: {
                    colors: ['#000'],
                    fontSize: '16px'
                }
            },
            legend: {
                show: true, // Menampilkan legend
                position: 'top', // Posisi legend ('top', 'bottom', 'left', 'right')
                horizontalAlign: 'center', // Penyusunan legend ('left', 'center', 'right')
                fontSize: '14px',
                markers: {
                    width: 12,
                    height: 12,
                    radius: 12 // Membuat marker berbentuk lingkaran
                },
                itemMargin: {
                    horizontal: 10,
                    vertical: 5
                }
            },
            grid: {
                strokeDashArray: 8,
                borderColor,
                padding: {
                    bottom: -10
                }
            },
            xaxis: {
                axisTicks: { show: false },
                crosshairs: { opacity: 0 },
                axisBorder: { show: false },
                categories: dailyDataMtf.labels, //['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                tickPlacement: 'on',
                labels: {
                    show: true,
                    //rotate: -45,
                    //rotateAlways: true,
                    style: {
                        fontSize: '16px',
                        fontWeight: '4'
                    }
                },
                axisBorder: {
                    show: false
                },
                axisTicks: {
                    show: false
                }
            },
            yaxis: {
                min: 0,
                max: Math.max(...dailyDataMit.counts.concat(dailyDataMtf.counts)) * 1.2,
                show: true,
                tickAmount: 3,
                labels: {
                    formatter: function (val) {
                        return parseInt(val);
                    },
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Inter',
                        colors: labelColor
                    }
                }
            },
            states: {
                hover: {
                    filter: {
                        type: 'none'
                    }
                },
                active: {
                    filter: {
                        type: 'none'
                    }
                }
            },
            responsive: [
                {
                    breakpoint: 1500,
                    options: {
                        plotOptions: {
                            bar: {
                                columnWidth: '80%'
                            }
                        }
                    }
                },
                {
                    breakpoint: 1200,
                    options: {
                        plotOptions: {
                            bar: {
                                columnWidth: '30%'
                            }
                        }
                    }
                },
                {
                    breakpoint: 815,
                    options: {
                        plotOptions: {
                            bar: {
                                borderRadius: 5
                            }
                        }
                    }
                },
                {
                    breakpoint: 768,
                    options: {
                        plotOptions: {
                            bar: {
                                borderRadius: 10,
                                columnWidth: '20%'
                            }
                        }
                    }
                },
                {
                    breakpoint: 568,
                    options: {
                        plotOptions: {
                            bar: {
                                borderRadius: 8,
                                columnWidth: '30%'
                            }
                        }
                    }
                },
                {
                    breakpoint: 410,
                    options: {
                        plotOptions: {
                            bar: {
                                columnWidth: '50%'
                            }
                        }
                    }
                }
            ]
        };
        if (weeklyOverviewChartEl) {
            const weeklyOverviewChart = new ApexCharts(weeklyOverviewChartEl, weeklyOverviewChartConfig);
            weeklyOverviewChart.render();
        }
    }

    $.ajax({
        url: assetsPath + 'json/BackDataDone-Mit.json',     //'/Home/GetDataPengecekan',
        method: "GET",//'POST',
        dataType: 'json',
        //data: {
        //    User_Pengaju: '',
        //    User_Group: '',
        //    tipw: '',
        //    status: '40',
        //    startDate: '',
        //    endDate: ''
        //},
        success: function (response) {
            if (response && Array.isArray(response)) {
                $.ajax({
                    url: assetsPath + 'json/BackDataDone-Mtf_.json',
                    method: "GET",
                    dataType: 'json',
                    success: function (responseMtf) {
                        console.log(responseMtf);
                        const dailyDataMit = countDataPerDay(response);
                        const monthlyDataMit = countDataPerMonth(response);
                        const jenisBadanDataMit = countDataPerMonthJenisBadan(response);
                        const dailyDataMtf = countDataPerDay(responseMtf);
                        const monthlyDataMtf = countDataPerMonth(responseMtf);
                        const jenisBadanDataMtf = countDataPerMonthJenisBadan(responseMtf);
                        const GroupJenisBadanMTF = countDataPerGropMont(responseMtf);
                        const GroupJenisBadanMitsui = countDataPerGropMont(response);
                        const jenisBadanData = countDataPerMonthJenisBadan(responseMtf);
                        const jenisBadanDataMit2 = countDataPerMonthJenisBadan(response);

                        GroupJenisBadanMTF.labels.forEach(label => {
                            console.log(`\n${label}:`);
                            console.log('PT:', GroupJenisBadanMTF.data[label].PT);
                            console.log('CV:', GroupJenisBadanMTF.data[label].CV);
                            console.log('Lainnya:', GroupJenisBadanMTF.data[label].Lainnya);
                        });

                        initializeChart(dailyDataMit, dailyDataMtf);
                        initializeChartMonth(monthlyDataMit, monthlyDataMtf);
                        initializeChartJenisBadan(jenisBadanData);
                        initializeChartJenisBadanMitsui(jenisBadanDataMit2);
                    }
                });
                // Hitung jumlah data selama 1 minggu terakhir dibagi per hari
                //const dailyData = countDataPerDay(response);
                //const monthlyData = countDataPerMonth(response);
                const jenisBadanData = countDataPerMonthJenisBadan(response);
                //console.log('Data harian:', dailyData);
                //console.log('Kelompok:', jenisBadanData);

                //// Inisialisasi grafik dengan data yang diperoleh
                //initializeChart(dailyData);
                //initializeChartMonth(monthlyData);
                //initializeChartJenisBadan(jenisBadanData);
            } else {
                console.error('Response tidak sesuai format.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Gagal mengambil data:', error);
        }
    });

    // Total Profit line chart
    // --------------------------------------------------------------------
    const totalProfitLineChartEl = document.querySelector('#totalProfitLineChart'),
        totalProfitLineChartConfig = {
            chart: {
                height: 90,
                type: 'line',
                parentHeightOffset: 0,
                toolbar: {
                    show: false
                }
            },
            grid: {
                borderColor: labelColor,
                strokeDashArray: 6,
                xaxis: {
                    lines: {
                        show: true
                    }
                },
                yaxis: {
                    lines: {
                        show: false
                    }
                },
                padding: {
                    top: -15,
                    left: -7,
                    right: 9,
                    bottom: -15
                }
            },
            colors: [config.colors.primary],
            stroke: {
                width: 3
            },
            series: [
                {
                    data: [0, 20, 5, 30, 15, 45]
                }
            ],
            tooltip: {
                shared: false,
                intersect: true,
                x: {
                    show: false
                }
            },
            xaxis: {
                labels: {
                    show: false
                },
                axisTicks: {
                    show: false
                },
                axisBorder: {
                    show: false
                }
            },
            yaxis: {
                labels: {
                    show: false
                }
            },
            tooltip: {
                enabled: false
            },
            markers: {
                size: 6,
                strokeWidth: 3,
                strokeColors: 'transparent',
                strokeWidth: 3,
                colors: ['transparent'],
                discrete: [
                    {
                        seriesIndex: 0,
                        dataPointIndex: 5,
                        fillColor: cardColor,
                        strokeColor: config.colors.primary,
                        size: 6,
                        shape: 'circle'
                    }
                ],
                hover: {
                    size: 7
                }
            },
            responsive: [
                {
                    breakpoint: 1350,
                    options: {
                        chart: {
                            height: 80
                        }
                    }
                },
                {
                    breakpoint: 1200,
                    options: {
                        chart: {
                            height: 100
                        }
                    }
                },
                {
                    breakpoint: 768,
                    options: {
                        chart: {
                            height: 110
                        }
                    }
                }
            ]
        };
    if (typeof totalProfitLineChartEl !== undefined && totalProfitLineChartEl !== null) {
        const totalProfitLineChart = new ApexCharts(totalProfitLineChartEl, totalProfitLineChartConfig);
        totalProfitLineChart.render();
    }

    // Sessions Column Chart
    // --------------------------------------------------------------------
    const sessionsColumnChartEl = document.querySelector('#sessionsColumnChart'),
        sessionsColumnChartConfig = {
            chart: {
                height: 90,
                parentHeightOffset: 0,
                type: 'bar',
                toolbar: {
                    show: false
                }
            },
            tooltip: {
                enabled: false
            },
            plotOptions: {
                bar: {
                    barHeight: '100%',
                    columnWidth: '20px',
                    startingShape: 'rounded',
                    endingShape: 'rounded',
                    borderRadius: 4,
                    colors: {
                        ranges: [
                            {
                                from: 25,
                                to: 32,
                                color: config.colors.danger
                            },
                            {
                                from: 60,
                                to: 75,
                                color: config.colors.primary
                            },
                            {
                                from: 45,
                                to: 50,
                                color: config.colors.danger
                            },
                            {
                                from: 35,
                                to: 42,
                                color: config.colors.primary
                            }
                        ],
                        backgroundBarColors: [chartBgColor, chartBgColor, chartBgColor, chartBgColor, chartBgColor],
                        backgroundBarRadius: 4
                    }
                }
            },
            grid: {
                show: false,
                padding: {
                    top: -10,
                    left: -10,
                    bottom: -15
                }
            },
            dataLabels: {
                enabled: false
            },
            legend: {
                show: false
            },
            xaxis: {
                labels: {
                    show: false
                },
                axisTicks: {
                    show: false
                },
                axisBorder: {
                    show: false
                }
            },
            yaxis: {
                labels: {
                    show: false
                }
            },
            series: [
                {
                    data: [30, 70, 50, 40, 60]
                }
            ],
            responsive: [
                {
                    breakpoint: 1350,
                    options: {
                        chart: {
                            height: 80
                        },
                        plotOptions: {
                            bar: {
                                columnWidth: '40%'
                            }
                        }
                    }
                },
                {
                    breakpoint: 1200,
                    options: {
                        chart: {
                            height: 100
                        },
                        plotOptions: {
                            bar: {
                                columnWidth: '20%'
                            }
                        }
                    }
                },
                {
                    breakpoint: 768,
                    options: {
                        chart: {
                            height: 110
                        },
                        plotOptions: {
                            bar: {
                                columnWidth: '10%'
                            }
                        }
                    }
                },
                {
                    breakpoint: 480,
                    options: {
                        plotOptions: {
                            bar: {
                                columnWidth: '20%'
                            }
                        }
                    }
                }
            ]
        };
    if (typeof sessionsColumnChartEl !== undefined && sessionsColumnChartEl !== null) {
        const sessionsColumnChart = new ApexCharts(sessionsColumnChartEl, sessionsColumnChartConfig);
        sessionsColumnChart.render();
    }

    // Performance Radar Chart
    // --------------------------------------------------------------------
    function initializeChartJenisBadan2(monthlyDataMitsui, monthlyDataMandiri) {
        console.log('Mitsui' + monthlyDataMitsui);
        console.log('Mandiri' + monthlyDataMandiri);
        const combinedLabels = [...new Set([...monthlyDataMitsui.labels, ...monthlyDataMandiri.labels])];
        console.log(combinedLabels)
        function alignData(labels, dataset) {
            // Asumsikan dataset memiliki struktur baru: 
            // labels: ['Label1', 'Label2', ...]
            // data: {
            //   'Label1': [dataX, dataY, dataZ],
            //   'Label2': [dataX, dataY, dataZ]
            // }
            return labels.map(label =>
                dataset.data[label] || [0, 0, 0]
            );
        }
        const alignedMitsuiData = alignData(combinedLabels, monthlyDataMitsui);
        const alignedMandiriData = alignData(combinedLabels, monthlyDataMandiri);

        const performanceChartConfig = {
            chart: {
                type: 'bar',
                height: 300,
                stacked: true, // Gunakan stacked bar untuk menampilkan 3 data dalam satu bar
                stackType: '100%', // Opsional: jika ingin menampilkan persentase
                offsetY: -9,
                offsetX: -16,
                parentHeightOffset: 0,
                toolbar: {
                    show: false
                }
            },
            series: [
                {
                    name: 'Mitsui - Data X',
                    data: alignedMitsuiData.map(data => data[0])
                },
                {
                    name: 'Mitsui - Data Y',
                    data: alignedMitsuiData.map(data => data[1])
                },
                {
                    name: 'Mitsui - Data Z',
                    data: alignedMitsuiData.map(data => data[2])
                },
                {
                    name: 'MTF - Data X',
                    data: alignedMandiriData.map(data => data[0])
                },
                {
                    name: 'MTF - Data Y',
                    data: alignedMandiriData.map(data => data[1])
                },
                {
                    name: 'MTF - Data Z',
                    data: alignedMandiriData.map(data => data[2])
                }
            ],
            colors: [
                '#9694FF', '#6c48c5', '#4a3a8c', // Warna untuk Mitsui
                '#FF6384', '#36A2EB', '#FFCE56'  // Warna untuk MTF
            ],
            plotOptions: {
                bar: {
                    borderRadius: 4,
                    columnWidth: '50%',
                    endingShape: 'rounded',
                    startingShape: 'rounded',
                }
            },
            dataLabels: {
                enabled: true,
                formatter: function (val) {
                    return val > 0 ? val : '';
                },
                style: {
                    colors: ['#000'],
                    fontSize: '12px'
                }
            },
            legend: {
                show: true,
                position: 'top',
                horizontalAlign: 'left',
                offsetX: 40
            },
            grid: {
                strokeDashArray: 8,
                borderColor: '#e0e0e0',
                padding: {
                    bottom: -10
                }
            },
            xaxis: {
                categories: combinedLabels,
                axisTicks: { show: false },
                crosshairs: { opacity: 0 },
                axisBorder: { show: false },
                tickPlacement: 'on',
                labels: {
                    show: true,
                    style: {
                        fontSize: '16px',
                        fontWeight: '400'
                    }
                }
            },
            yaxis: {
                min: 0,
                max: 100, // Sesuaikan dengan kebutuhan Anda
                show: true,
                tickAmount: 3,
                labels: {
                    formatter: function (val) {
                        return parseInt(val);
                    },
                    style: {
                        fontSize: '14px',
                        fontFamily: 'Inter'
                    }
                }
            },
            fill: {
                opacity: 1
            },
            tooltip: {
                // Konfigurasi tooltip untuk menampilkan detail setiap bagian
                shared: true,
                intersect: false
            }
        };
        if (performanceChartEl) {
            const performanceChart = new ApexCharts(performanceChartEl, performanceChartConfig);
            performanceChart.render();
        }
    }

    function initializeChartJenisBadannnnnn(jenisBadanDataMit, jenisBadanDataMtf) {
        const combinedLabels = [...new Set([...jenisBadanDataMit.labels, ...jenisBadanDataMtf.labels])];

        // Fungsi untuk menyelaraskan data berdasarkan label gabungan
        function alignData(labels, dataset) {
            const dataMap = Object.fromEntries(dataset.labels.map((label, index) => [label, dataset.counts[index]]));
            return labels.map(label => dataMap[label] || 0); // Isi dengan 0 jika label tidak ditemukan
        }

        // Selaraskan data Mitsui dan Mandiri
        const alignedMitsuiData = {
            countsCV: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsCV }),
            countsPT: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsPT }),
            countsElse: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsElse }),
        };

        const alignedMandiriData = {
            countsCV: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsCV }),
            countsPT: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsPT }),
            countsElse: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsElse }),
        };

        const performanceChartEl = document.querySelector('#performanceChart'); // Sesuaikan dengan ID elemen Anda

        if (performanceChartEl) {
            const performanceChartConfig = {
                chart: {
                    type: 'bar',
                    height: 400,
                    stacked: true, // Aktifkan stacking
                    toolbar: {
                        show: true
                    },
                    stackType: 'normal' // Stack data secara normal
                },
                series: [
                    {
                        name: 'CV - Mitsui',
                        data: alignedMitsuiData.countsCV
                    },
                    {
                        name: 'PT - Mitsui',
                        data: alignedMitsuiData.countsPT
                    },
                    {
                        name: 'Lainnya - Mitsui',
                        data: alignedMitsuiData.countsElse
                    },
                    {
                        name: 'CV - Mandiri',
                        data: alignedMandiriData.countsCV
                    },
                    {
                        name: 'PT - Mandiri',
                        data: alignedMandiriData.countsPT
                    },
                    {
                        name: 'Lainnya - Mandiri',
                        data: alignedMandiriData.countsElse
                    }
                ],
                xaxis: {
                    categories: combinedLabels, // Label gabungan
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontFamily: 'Inter',
                            fontWeight: 400,
                            colors: '#6c757d'
                        }
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: '30%', // Ukuran lebar bar
                        dataLabels: {
                            position: 'center' // Tampilkan nilai di tengah bar
                        }
                    }
                },
                colors: ['#007bff', '#17a2b8', '#28a745', '#ffc107', '#dc3545', '#6c757d'],
                legend: {
                    position: 'bottom',
                    offsetY: 10,
                    itemMargin: {
                        horizontal: 8,
                        vertical: 8
                    },
                    fontSize: '14px',
                    fontFamily: 'Inter'
                },
                fill: {
                    opacity: 1
                },
                grid: {
                    borderColor: '#e7e7e7',
                    row: {
                        colors: ['#f3f3f3', 'transparent'], // Warna garis grid
                        opacity: 0.5
                    }
                },
                tooltip: {
                    shared: true, // Tooltip gabungan per label
                    intersect: false
                },
                dataLabels: {
                    enabled: false
                }
            };

            const performanceChart = new ApexCharts(performanceChartEl, performanceChartConfig);
            performanceChart.render();
        }
    }


    function initializeChartJenisBadanOld22(jenisBadanDataMit, jenisBadanDataMtf) {
        const combinedLabels = [...new Set([...jenisBadanDataMit.labels, ...jenisBadanDataMtf.labels])];

        // Fungsi untuk menyelaraskan data berdasarkan label gabungan
        function alignData(labels, dataset) {
            const dataMap = Object.fromEntries(dataset.labels.map((label, index) => [label, dataset.counts[index]]));
            return labels.map(label => dataMap[label] || 0); // Isi dengan 0 jika label tidak ditemukan
        }

        // Selaraskan data Mitsui dan Mandiri
        const alignedMitsuiData = {
            countsCV: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsCV }),
            countsPT: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsPT }),
            countsElse: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsElse }),
        };

        const alignedMandiriData = {
            countsCV: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsCV }),
            countsPT: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsPT }),
            countsElse: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsElse }),
        };

        const performanceChartEl = document.querySelector('#performanceChart2'); // Sesuaikan dengan ID elemen Anda

        if (performanceChartEl) {
            const performanceChartConfig = {
                chart: {
                    type: 'bar',
                    height: 400,
                    stacked: true, // Aktifkan stacking
                    toolbar: {
                        show: true
                    }
                },
                series: [
                    {
                        name: 'CV - Mitsui',
                        data: alignedMitsuiData.countsCV
                    },
                    {
                        name: 'PT - Mitsui',
                        data: alignedMitsuiData.countsPT
                    },
                    {
                        name: 'Lainnya - Mitsui',
                        data: alignedMitsuiData.countsElse
                    },
                    {
                        name: 'CV - Mandiri',
                        data: alignedMandiriData.countsCV
                    },
                    {
                        name: 'PT - Mandiri',
                        data: alignedMandiriData.countsPT
                    },
                    {
                        name: 'Lainnya - Mandiri',
                        data: alignedMandiriData.countsElse
                    }
                ],
                xaxis: {
                    categories: combinedLabels, // Label gabungan
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontFamily: 'Inter',
                            fontWeight: 400,
                            colors: '#6c757d'
                        }
                    }
                },
                yaxis: {
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontFamily: 'Inter'
                        }
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: '50%', // Ukuran lebar bar
                        dataLabels: {
                            position: 'center' // Tampilkan nilai di tengah bar
                        }
                    }
                },
                colors: ['#007bff', '#17a2b8', '#28a745', '#ffc107', '#dc3545', '#6c757d'],
                legend: {
                    position: 'bottom',
                    offsetY: 10,
                    itemMargin: {
                        horizontal: 8,
                        vertical: 8
                    },
                    fontSize: '14px',
                    fontFamily: 'Inter'
                },
                fill: {
                    opacity: 1
                },
                grid: {
                    borderColor: '#e7e7e7',
                    row: {
                        colors: ['#f3f3f3', 'transparent'], // Warna garis grid
                        opacity: 0.5
                    }
                }
            };

            const performanceChart = new ApexCharts(performanceChartEl, performanceChartConfig);
            performanceChart.render();
        }
    }
    function initializeChartJenisBadan(JenisBadan) {
        const performanceChartConfig = {
            chart: {
                height: 450,
                fontFamily: 'Inter',
                type: 'bar',
                stacked: true,
                parentHeightOffset: 0,
                toolbar: {
                    show: false
                }
            },
            plotOptions: {
                bar: {
                    columnWidth: '15%',
                    colors: {
                        backgroundBarColors: [
                            chartColors.column.bg,
                            chartColors.column.bg,
                            chartColors.column.bg,
                            chartColors.column.bg,
                            chartColors.column.bg
                        ],
                        backgroundBarRadius: 10
                    }
                }
            },
            dataLabels: {
                enabled: false
            },
            legend: {
                show: true,
                position: 'top',
                horizontalAlign: 'start',
                fontSize: '13px',
                markers: {
                    width: 10,
                    height: 10
                },
                labels: {
                    colors: legendColor,
                    useSeriesColors: false
                }
            },
            colors: [chartColors.column.series1, chartColors.column.series2, chartColors.column.series3],
            stroke: {
                show: true,
                colors: ['transparent']
            },
            grid: {
                borderColor: borderColor,
                xaxis: {
                    lines: {
                        show: true
                    }
                }
            },
            series: [
                {
                    name: 'Data CV Cek',
                    data: JenisBadan.countsCV
                },
                {
                    name: 'Data PT Cek',
                    data: JenisBadan.countsPT
                },
                {
                    name: 'Data Selain PT & CV',
                    data: JenisBadan.countsElse
                }
            ],
            xaxis: {
                categories: JenisBadan.labels,//['7/12', '8/12', '9/12', '10/12', '11/12', '12/12', '13/12', '14/12', '15/12', '16/12'],
                axisBorder: {
                    show: false
                },
                axisTicks: {
                    show: false
                },
                labels: {
                    style: {
                        colors: labelColor,
                        fontSize: '13px'
                    }
                }
            },
            yaxis: {
                labels: {
                    style: {
                        colors: labelColor,
                        fontSize: '13px'
                    }
                }
            },
            fill: {
                opacity: 1
            }
        };
        if (performanceChartEl) {
            const performanceChart = new ApexCharts(performanceChartEl, performanceChartConfig);
            performanceChart.render();
        }
    }

    function initializeChartJenisBadanMitsui(JenisBadan) {
        const performanceChartConfig = {
            chart: {
                height: 450,
                fontFamily: 'Inter',
                type: 'bar',
                stacked: true,
                parentHeightOffset: 0,
                toolbar: {
                    show: false
                }
            },
            plotOptions: {
                bar: {
                    columnWidth: '15%',
                    colors: {
                        backgroundBarColors: [
                            chartColors.column.bg,
                            chartColors.column.bg,
                            chartColors.column.bg,
                            chartColors.column.bg,
                            chartColors.column.bg
                        ],
                        backgroundBarRadius: 10
                    }
                }
            },
            dataLabels: {
                enabled: false
            },
            legend: {
                show: true,
                position: 'top',
                horizontalAlign: 'start',
                fontSize: '13px',
                markers: {
                    width: 10,
                    height: 10
                },
                labels: {
                    colors: legendColor,
                    useSeriesColors: false
                }
            },
            colors: [chartColors.column.series1, chartColors.column.series2, chartColors.column.series3],
            stroke: {
                show: true,
                colors: ['transparent']
            },
            grid: {
                borderColor: borderColor,
                xaxis: {
                    lines: {
                        show: true
                    }
                }
            },
            series: [
                {
                    name: 'Data CV Cek',
                    data: JenisBadan.countsCV
                },
                {
                    name: 'Data PT Cek',
                    data: JenisBadan.countsPT
                },
                {
                    name: 'Data Selain PT & CV',
                    data: JenisBadan.countsElse
                }
            ],
            xaxis: {
                categories: JenisBadan.labels,//['7/12', '8/12', '9/12', '10/12', '11/12', '12/12', '13/12', '14/12', '15/12', '16/12'],
                axisBorder: {
                    show: false
                },
                axisTicks: {
                    show: false
                },
                labels: {
                    style: {
                        colors: labelColor,
                        fontSize: '13px'
                    }
                }
            },
            yaxis: {
                labels: {
                    style: {
                        colors: labelColor,
                        fontSize: '13px'
                    }
                }
            },
            fill: {
                opacity: 1
            }
        };
        if (performanceChartMit) {
            const performanceChart = new ApexCharts(performanceChartMit, performanceChartConfig);
            performanceChart.render();
        }
    }

    function initializeChartJenisBadanold(jenisBadanDataMit, jenisBadanDataMtf) {
        const combinedLabels = [...new Set([...jenisBadanDataMit.labels, ...jenisBadanDataMtf.labels])];

        // Fungsi untuk menyelaraskan data berdasarkan label gabungan
        function alignData(labels, dataset) {
            const dataMap = Object.fromEntries(dataset.labels.map((label, index) => [label, dataset.counts[index]]));
            return labels.map(label => dataMap[label] || 0); // Isi dengan 0 jika label tidak ditemukan
        }

        // Selaraskan data Mitsui dan Mandiri
        const alignedMitsuiData = {
            countsCV: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsCV }),
            countsPT: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsPT }),
            countsElse: alignData(combinedLabels, { labels: jenisBadanDataMit.labels, counts: jenisBadanDataMit.countsElse }),
        };

        const alignedMandiriData = {
            countsCV: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsCV }),
            countsPT: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsPT }),
            countsElse: alignData(combinedLabels, { labels: jenisBadanDataMtf.labels, counts: jenisBadanDataMtf.countsElse }),
        };

        const performanceChartEl = document.querySelector('#performanceChart'); // Sesuaikan dengan ID elemen Anda

        if (performanceChartEl) {
            const performanceChartConfig = {
                chart: {
                    type: 'bar',
                    height: 400,
                    stacked: true,
                    toolbar: {
                        show: true
                    }
                },
                series: [
                    {
                        name: 'CV - Mitsui',
                        data: alignedMitsuiData.countsCV
                    },
                    {
                        name: 'CV - Mandiri',
                        data: alignedMandiriData.countsCV
                    },
                    {
                        name: 'PT - Mitsui',
                        data: alignedMitsuiData.countsPT
                    },
                    {
                        name: 'PT - Mandiri',
                        data: alignedMandiriData.countsPT
                    },
                    {
                        name: 'Lainnya - Mitsui',
                        data: alignedMitsuiData.countsElse
                    },
                    {
                        name: 'Lainnya - Mandiri',
                        data: alignedMandiriData.countsElse
                    }
                ],
                xaxis: {
                    categories: combinedLabels,
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontFamily: 'Inter',
                            fontWeight: 400,
                            colors: '#6c757d'
                        }
                    }
                },
                yaxis: {
                    labels: {
                        style: {
                            fontSize: '12px',
                            fontFamily: 'Inter'
                        }
                    }
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: '60%',
                        dataLabels: {
                            position: 'top'
                        }
                    }
                },
                colors: ['#007bff', '#17a2b8', '#28a745', '#ffc107', '#dc3545', '#6c757d'],
                legend: {
                    position: 'bottom',
                    offsetY: 10,
                    itemMargin: {
                        horizontal: 8,
                        vertical: 8
                    },
                    fontSize: '14px',
                    fontFamily: 'Inter'
                },
                fill: {
                    opacity: 1
                },
                grid: {
                    borderColor: '#e7e7e7',
                    row: {
                        colors: ['#f3f3f3', 'transparent'], // Warna garis grid
                        opacity: 0.5
                    }
                }
            };

            const performanceChart = new ApexCharts(performanceChartEl, performanceChartConfig);
            performanceChart.render();
        }
    }
})();
