#nullable enable

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarMaintenanceManager
{
    public class MaintenanceForm : Form
    {
        // Átnevezve, hogy ne rejtse el az öröklött Control.Created property-t
        public Maintenance? CreatedMaintenance { get; private set; }

        private readonly Car _car;

        private TextBox        txtType     = null!;
        private DateTimePicker dtpDate     = null!;
        private NumericUpDown  nudCost     = null!;
        private TextBox        txtMechanic = null!;
        private Button         btnSave     = null!;

        public MaintenanceForm(Car car)
        {
            _car = car;
            SetupDialog();
        }

        private void SetupDialog()
        {
            // Alap ablakbeállítások
            this.Text            = $"Új karbantartás: {_car.Display}";
            this.ClientSize      = new Size(400, 240);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.MaximizeBox     = false;

            // Típus
            var lblType = new Label {
                Text     = "Típus:",
                Location = new Point(10, 10),
                AutoSize = true
            };
            txtType = new TextBox {
                Location = new Point(100, 8),
                Width    = 280
            };

            // Dátum
            var lblDate = new Label {
                Text     = "Dátum:",
                Location = new Point(10, 50),
                AutoSize = true
            };
            dtpDate = new DateTimePicker {
                Location = new Point(100, 48),
                Width    = 200,
                Format   = DateTimePickerFormat.Short
            };

            // Költség
            var lblCost = new Label {
                Text     = "Költség (Ft):",
                Location = new Point(10, 90),
                AutoSize = true
            };
            nudCost = new NumericUpDown {
                Location           = new Point(100, 88),
                Width              = 120,
                Maximum            = 10_000_000,
                ThousandsSeparator = true
            };

            // Szerelő neve
            var lblMech = new Label {
                Text     = "Szerelő neve:",
                Location = new Point(10, 130),
                AutoSize = true
            };
            txtMechanic = new TextBox {
                Location = new Point(100, 128),
                Width    = 280
            };

            // Mentés gomb
            btnSave = new Button {
                Text         = "Mentés",
                Location     = new Point(150, 170),
                Width        = 100,
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            // Vezérlők hozzáadása
            this.Controls.AddRange(new Control[] {
                lblType, txtType,
                lblDate, dtpDate,
                lblCost, nudCost,
                lblMech, txtMechanic,
                btnSave
            });
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // A CreatedMaintenance nullable property mostantól helyesen beállítva
            CreatedMaintenance = new Maintenance {
                CarId     = _car.Id,
                Date      = dtpDate.Value.Date,
                Operation = txtType.Text,
                Note      = $"{nudCost.Value:N0} Ft ({txtMechanic.Text})"
            };
            this.Close();
        }
    }
}