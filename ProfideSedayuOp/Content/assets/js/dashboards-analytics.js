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
            series3:'#FFB200'
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

        sortedKeys.forEach(key => {
            const [year, month] = key.split('-').map(Number);
            // Hanya tambahkan bulan yang memiliki data
            if (monthlyCounts[key] > 0) {
                countsArray.push(monthlyCounts[key]);
                labelsArray.push(`${monthNames[month]} ${year}`);
            }
        });

        return {
            counts: countsArray,
            labels: labelsArray
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


    function initializeChartMonth(monthlyData) {
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
                    name: 'Jumlah Data',
                    data: monthlyData.counts
                }
            ],
            colors: [chartBgColor],
            plotOptions: {
                bar: {
                    borderRadius: 8,
                    columnWidth: '50%',
                    endingShape: 'rounded',
                    startingShape: 'rounded',
                    colors: {
                        ranges: [
                            {
                                from: 75,
                                to: 1000,
                                color: config.colors.primary
                            },
                            {
                                from: 0,
                                to: 73,
                                color: config.colors.primary
                            }
                        ]
                    }
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
                show: false
            },
            grid: {
                strokeDashArray: 8,
                borderColor,
                padding: {
                    bottom: -10
                }
            },
            xaxis: {
                categories: monthlyData.labels,
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
                        fontWeight : '4'
                    }
                }
            },
            yaxis: {
                min: 0,
                max: Math.max(...monthlyData.counts) * 1.2, // Dinamis sesuai data
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
    function initializeChart(dailyData) {
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
                    name: 'Jumlah Data',
                    data: dailyData.counts
                }
            ],
            colors: [chartBgColor],
            plotOptions: {
                bar: {
                    borderRadius: 8,
                    columnWidth: '30%',
                    endingShape: 'rounded',
                    startingShape: 'rounded',
                    colors: {
                        ranges: [
                            {
                                from: 75,
                                to: 80,
                                color: config.colors.primary
                            },
                            {
                                from: 0,
                                to: 73,
                                color: config.colors.primary
                            }
                        ]
                    }
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
                show: false
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
                categories: dailyData.labels, //['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                tickPlacement: 'on',
                labels: {
                    show: true,
                    //rotate: -45,
                    //rotateAlways: true,
                    style: {
                        fontSize: '16px',
                        fontWeight:'4'
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
                max: Math.max(...dailyData.counts) * 1.2,
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
                                columnWidth: '40%'
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
        url: assetsPath + 'json/BackDataDone.json',     //'/Home/GetDataPengecekan',
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
                // Hitung jumlah data selama 1 minggu terakhir dibagi per hari
                const dailyData = countDataPerDay(response);
                const monthlyData = countDataPerMonth(response);
                const jenisBadanData = countDataPerMonthJenisBadan(response);
                console.log('Data harian:', dailyData);
                console.log('Kelompok:', jenisBadanData);

                // Inisialisasi grafik dengan data yang diperoleh
                initializeChart(dailyData);
                initializeChartMonth(monthlyData);
                initializeChartJenisBadan(jenisBadanData);
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

    function initializeChartJenisBadan2(JenisBadan) {
        const performanceChartConfig = {
           
            
            chart: {
                height: 310,
                type: '',
                offsetY: 10,
                toolbar: {
                    show: false
                }
            },
            legend: {
                show: true,
                position: 'bottom',
                offsetY: 10,
                markers: {
                    width: 10,
                    height: 10,
                    offsetX: -2
                },
                itemMargin: {
                    horizontal: 10,
                    vertical: 8
                },
                fontFamily: 'Inter',
                fontSize: '15px',
                labels: {
                    colors: bodyColor,
                    useSeriesColors: false
                }
            },
            plotOptions: {
                radar: {
                    polygons: {
                        strokeColors: borderColor,
                        connectorColors: borderColor
                    }
                }
            },
            yaxis: {
                show: false
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
            colors: [config.colors.primary, config.colors.info, config.colors.success],
            xaxis: {
                categories: JenisBadan.labels,
                labels: {
                    show: true,
                    style: {
                        colors: [labelColor, labelColor, labelColor, labelColor, labelColor, labelColor],
                        fontSize: '13px',
                        fontFamily: 'Inter',
                        fontWeight: 400
                    }
                }
            },
            fill: {
                opacity: [1, 0.9]
            },
            stroke: {
                show: false,
                width: 0
            },
            markers: {
                size: 0
            },
            grid: {
                show: false,
                padding: {
                    bottom: -10
                }
            }
        };
        if (performanceChartEl) {
            const performanceChart = new ApexCharts(performanceChartEl, performanceChartConfig);
            performanceChart.render();
        }
    }

})();
