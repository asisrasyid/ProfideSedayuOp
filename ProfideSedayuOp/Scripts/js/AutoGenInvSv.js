class InvoiceNumberGenerator {
    constructor() {
        // Peta bulan Romawi
        this.romanMonths = {
            1: 'I', 2: 'II', 3: 'III', 4: 'IV', 5: 'V', 6: 'VI',
            7: 'VII', 8: 'VIII', 9: 'IX', 10: 'X', 11: 'XI', 12: 'XII'
        };

        // Inisialisasi data invoice
        this.invoiceData = {
            month: null,
            year: null,
            sequence: 1
        };
    }

    // Ambil data terakhir dari file BackDataInv.json
    async fetchLatestInvoiceData() {
        try {
            const response = await fetch(assetsPath + 'json/BackDataInv.json');
            const data = await response.json();
            // Ambil nomor invoice terakhir dari array
            if (data && data.length > 0) {
                const lastInvoice = data[data.length - 1];

                // Parse nomor invoice terakhir
                const lastInvoiceParts = lastInvoice.No_Invoice.split('/');
                const lastSequence = parseInt(lastInvoiceParts[0], 10);
                const lastRomanMonth = lastInvoiceParts[3];
                const lastYear = parseInt(lastInvoiceParts[4], 10);

                // Konversi bulan Romawi ke nomor
                const currentDate = new Date();
                const currentMonth = currentDate.getMonth() + 1;
                const currentYear = currentDate.getFullYear();

                // Cek apakah bulan dan tahun sama dengan invoice terakhir
                if (this.romanMonths[currentMonth] === lastRomanMonth && currentYear === lastYear) {
                    // Gunakan sequence berikutnya
                    this.invoiceData = {
                        month: currentMonth,
                        year: currentYear,
                        sequence: lastSequence + 1
                    };
                } else {
                    // Reset sequence jika beda bulan/tahun
                    this.invoiceData = {
                        month: currentMonth,
                        year: currentYear,
                        sequence: 1
                    };
                }
            } else {
                // Jika tidak ada data, gunakan sequence awal
                const currentDate = new Date();
                this.invoiceData = {
                    month: currentDate.getMonth() + 1,
                    year: currentDate.getFullYear(),
                    sequence: 1
                };
            }

            return this.invoiceData;
        } catch (error) {
            console.error('Error fetching invoice data:', error);

            // Fallback ke sequence awal jika gagal fetch
            const currentDate = new Date();
            this.invoiceData = {
                month: currentDate.getMonth() + 1,
                year: currentDate.getFullYear(),
                sequence: 1
            };
            return this.invoiceData;
        }
    }

    // Generate nomor invoice
    async generateInvoiceNumber() {
        // Pastikan data sudah diambil
        await this.fetchLatestInvoiceData();

        const currentDate = new Date();
        const currentMonth = currentDate.getMonth() + 1;
        const currentYear = currentDate.getFullYear();

        // Format nomor urut 4 digit
        const sequenceStr = this.invoiceData.sequence.toString().padStart(4, '0');

        // Buat nomor invoice
        const invoiceNumber = `${sequenceStr}/INV/SDB/${this.romanMonths[currentMonth]}/${currentYear}`;

        return invoiceNumber;
    }
}

// Fungsi untuk generate dan tampilkan invoice
async function generateInvoice() {
    const invoiceGenerator = new InvoiceNumberGenerator();

    try {
        // Ambil nomor invoice
        const invoiceNumber = await invoiceGenerator.generateInvoiceNumber();

        // Temukan input invoice
        const invoiceInput = document.getElementById('NoInvoice');

        // Tampilkan nomor invoice di input
        if (invoiceInput) {
            invoiceInput.value = invoiceNumber;
        } else {
            console.error('Input dengan ID "invoiceInp" tidak ditemukan');
        }

        return invoiceNumber;
    } catch (error) {
        console.error('Gagal generate invoice:', error);
    }
}

// Tambahkan event listener untuk generate invoice
document.addEventListener('DOMContentLoaded', () => {
    // Misalnya, generate invoice saat halaman selesai dimuat
    generateInvoice();
});

//// Export untuk penggunaan di modul
//export { InvoiceNumberGenerator, generateInvoice };