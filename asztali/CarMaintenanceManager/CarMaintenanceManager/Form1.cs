#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;  // a Font és Color miatt

namespace CarMaintenanceManager
{
    public partial class Form1 : Form
    {
        private ComboBox     cmbCars      = null!;
        private ListBox      lstRecords   = null!;
        private Button       btnNew       = null!;
        private Font         defaultFont  = new Font("Segoe UI", 10F);

        private List<Car>         cars    = new();
        private List<Maintenance> records = new();

        private const string CarsFile     = "cars.csv";
        private const string BookingsFile = "bookings.csv";
        private const string MaintFile    = "maintenances.csv";

        public Form1()
        {
            InitializeComponent();  
            SetupLayout();
            LoadCars();
            LoadRecords();
            RefreshDisplay();
        }

        private void SetupLayout()
        {
            // Form stílusa
            this.Text       = "Karbantartás Kezelő";
            this.Font       = defaultFont;
            this.BackColor  = Color.White;
            this.ClientSize = new Size(600, 400);

            // Felső panel + ComboBox
            var pnlTop = new Panel {
                Dock      = DockStyle.Top,
                Height    = 30,
                BackColor = Color.WhiteSmoke
            };
            cmbCars = new ComboBox {
                Dock            = DockStyle.Fill,
                DropDownStyle   = ComboBoxStyle.DropDownList,
                Font            = defaultFont
            };
            cmbCars.SelectedIndexChanged += (_,_) => RefreshDisplay();
            pnlTop.Controls.Add(cmbCars);
            this.Controls.Add(pnlTop);

            // Középső ListBox
            lstRecords = new ListBox {
                Dock        = DockStyle.Fill,
                Font        = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.None
            };
            this.Controls.Add(lstRecords);

            // Alsó panel + gomb
            var pnlBot = new Panel {
                Dock      = DockStyle.Bottom,
                Height    = 40,
                BackColor = Color.WhiteSmoke
            };
            btnNew = new Button {
                Text       = "Új karbantartás",
                Dock       = DockStyle.Fill,
                FlatStyle  = FlatStyle.Flat,
                Font       = defaultFont
            };
            btnNew.FlatAppearance.BorderSize = 0;
            btnNew.Click += BtnNew_Click;
            pnlBot.Controls.Add(btnNew);
            this.Controls.Add(pnlBot);
        }

        private void LoadCars()
        {
            if (!File.Exists(CarsFile)) return;
            var lines = File.ReadAllLines(CarsFile);
            foreach (var line in lines.Skip(1))
            {
                var p = line.Split(';');
                if (p.Length < 4) continue;
                cars.Add(new Car {
                    Id    = int.Parse(p[0]),
                    Make  = p[1],
                    Model = p[2],
                    Year  = int.Parse(p[3])
                });
            }
            cmbCars.DataSource    = cars;
            cmbCars.DisplayMember = "Display";
        }

        private void LoadRecords()
        {
            var path = File.Exists(MaintFile) ? MaintFile : BookingsFile;
            if (!File.Exists(path)) return;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines.Skip(1))
            {
                var p = line.Split(';');
                if (p.Length < 5) continue;
                records.Add(new Maintenance {
                    Id        = int.Parse(p[0]),
                    CarId     = int.Parse(p[1]),
                    Date      = DateTime.ParseExact(
                                   p[2], "yyyy-MM-dd",
                                   CultureInfo.InvariantCulture),
                    Operation = p[3],
                    Note      = p[4]
                });
            }
        }

        private void RefreshDisplay()
        {
            lstRecords.Items.Clear();
            if (cmbCars.SelectedItem is Car car)
            {
                foreach (var r in records
                                    .Where(r=>r.CarId==car.Id)
                                    .OrderBy(r=>r.Date))
                {
                    lstRecords.Items.Add(
                        $"{r.Date:yyyy-MM-dd} - {r.Operation} - {r.Note}"
                    );
                }
            }
        }

       private void BtnNew_Click(object? sender, EventArgs e)
{
    // Ellenőrizzük, hogy autó ki van-e választva
    if (cmbCars.SelectedItem is not Car car) 
        return;

    // Megnyitjuk a dialógot
    using var dlg = new MaintenanceForm(car);

    // Ha OK-val zárult ÉS létrejött egy Maintenance objektum…
    if (dlg.ShowDialog() == DialogResult.OK && dlg.CreatedMaintenance is Maintenance m)
    {
        // Id generálása
        m.Id = records.Any() ? records.Max(x => x.Id) + 1 : 1;

        // Listához adás és frissítés
        records.Add(m);
        RefreshDisplay();
    }
}
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            using var w = new StreamWriter(MaintFile, false);
            w.WriteLine("id;carId;date;operation;note");
            foreach (var r in records)
                w.WriteLine(
                    $"{r.Id};{r.CarId};{r.Date:yyyy-MM-dd};" +
                    $"{r.Operation};{r.Note}"
                );
        }
    }
}