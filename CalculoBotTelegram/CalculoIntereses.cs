using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CalculoBotTelegram
{
    public partial class CalculoIntereses : Form
    {
        List<ColeccionExpiraciones> listaExpiraciones = new List<ColeccionExpiraciones>();
        public CalculoIntereses()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listaExpiraciones.Clear();
            ProcesarDatos();

            /* foreach (var colx in listaExpiraciones)
            {
                MessageBox.Show(colx.NumeroDia + @" " + colx.Monto);
            }*/
        }

        private void ProcesarDatos()
        {
            dataGridView1.Rows.Clear();
            double montoInicial = Convert.ToDouble(textBox2.Text);
            double porcentaje = Convert.ToDouble(textBox3.Text)/100;
            double importeAcumulado = 0;
            double invertido = montoInicial;
            double importeReinversion = Convert.ToDouble(textBox4.Text);
            int numDiaActual;

            ColeccionExpiraciones col = new ColeccionExpiraciones();
            col.NumeroDia = Convert.ToInt32( textBox5.Text);
            col.Monto = montoInicial;
            col.DiaAplicacion = 0;

            listaExpiraciones.Add(col);

            for (int contx = 0; contx < Convert.ToInt32(textBox1.Text); contx++)
            {
                numDiaActual = contx + 1;
                dataGridView1.Rows.Add(numDiaActual, invertido, porcentaje*100, "0", "0");

                var pagodiarioCalculado = invertido*porcentaje;
                importeAcumulado += pagodiarioCalculado;
                dataGridView1["PagoDiario", dataGridView1.RowCount - 1].Value = pagodiarioCalculado;
                dataGridView1["Acumulado", dataGridView1.RowCount - 1].Value = importeAcumulado;

                foreach (var colx in listaExpiraciones)
                    // Comparar la lista de expiraciones para ver si el dia destino se alcanzo
                {
                    if (colx.NumeroDia == numDiaActual)
                    {
                        dataGridView1["Vencimiento", dataGridView1.RowCount - 1].Value = 
                            Convert.ToString(dataGridView1["Vencimiento", dataGridView1.RowCount - 1].Value).Length > 0 ?
                             (Convert.ToDouble( dataGridView1["Vencimiento", dataGridView1.RowCount - 1].Value) + colx.Monto) : colx.Monto;
                        invertido -= colx.Monto;
                    }

                    if (colx.DiaAplicacion == numDiaActual)
                    {
                        dataGridView1["Compra", dataGridView1.RowCount - 1].Value = colx.Monto;
                        invertido += colx.Monto;
                    }

                }


                if (importeAcumulado > importeReinversion)
                {
                    //Acumular importes de reinversion y agregar reinversion a la lista de expiraciones
                    int resEntero = Convert.ToInt32(importeAcumulado*100)/Convert.ToInt32(importeReinversion*100);
                    double nuevoImporteReinversion = importeReinversion*resEntero;
                    dataGridView1["Reinversion", dataGridView1.RowCount - 1].Value = nuevoImporteReinversion;
                    importeAcumulado = importeAcumulado - nuevoImporteReinversion;
                    invertido += nuevoImporteReinversion;

                    ColeccionExpiraciones coln = new ColeccionExpiraciones(); // Agregar reinversion a la lista de expiraciones
                    coln.NumeroDia = numDiaActual + Convert.ToInt32(textBox5.Text); 
                    coln.Monto = nuevoImporteReinversion;
                    coln.DiaAplicacion = 0;
                    listaExpiraciones.Add(coln);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e.KeyCode == Keys.Escape)
                Close();
        }

        private void CalculoIntereses_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listaExpiraciones.Clear();
            for (int contx = 0; contx < dataGridView1.RowCount; contx++)
            {
                if (Convert.ToString(dataGridView1["Compra", contx].Value).Length > 0)
                {
                    ColeccionExpiraciones coln = new ColeccionExpiraciones();
                    coln.NumeroDia = Convert.ToInt32(dataGridView1["Dia", contx].Value) + Convert.ToInt32(textBox5.Text); 
                    coln.Monto = Convert.ToDouble(dataGridView1["Compra", contx].Value);
                    coln.DiaAplicacion = Convert.ToInt32(dataGridView1["Dia", contx].Value);
                    listaExpiraciones.Add(coln);
                 }
            }
            ProcesarDatos();
        }
    }


    class ColeccionExpiraciones
    {
        public int NumeroDia;
        public double Monto;
        public int DiaAplicacion;
    }
}
