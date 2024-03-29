﻿using AppCore.Factories;
using AppCore.IServices;
using AppCore.Services;
using Autofac;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.Shared;
using Infraestructure.Repository;
using practicaDepreciacion.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practicaDepreciacion
{
    public partial class Form1 : Form
    {
        IActivoServices activoServices;
        IEmpleadoServices empleadoServices;
        private static List<Empleado> empleados;
        static AutoCompleteStringCollection colection;
        public Form1(IActivoServices ActivoServices, IEmpleadoServices empleadoServices)
        {
            this.activoServices = ActivoServices;
            this.empleadoServices = empleadoServices;
            colection = new AutoCompleteStringCollection();
            empleados = empleadoServices.Read();
            InitializeComponent();
            empleadoAutocomplete();
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede numeros");
            }
        }

        private void txtValor_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtValorR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtVidaU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtEnviar_Click(object sender, EventArgs e)
        {
            if (verificar())
            {
                Activo activo = new Activo()
                {
                    Nombre = txtNombre.Text,
                    Valor = double.Parse(txtValor.Text),
                    ValorResidual = double.Parse(txtValorR.Text),
                    VidaUtil = int.Parse(txtVidaU.Text)
                };
                activoServices.Add(activo);
                //        dataGridView1.DataSource = null;
                limpiar();
                dataGridView1.DataSource = activoServices.Read();
            }
            else
            {
                MessageBox.Show("Tienes que llenar todos los formularios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool verificar()
        {
            string[] campos = { txtNombre.Text, txtValor.Text, txtVidaU.Text, txtValorR.Text };
            if (StringHelper.Wspaces(campos))
            {
                return false;
            }
            return true;
        }
        private void limpiar()
        {
            this.txtNombre.Text = String.Empty;
            this.txtValor.Text = String.Empty;
            this.txtValorR.Text = String.Empty;
            this.txtVidaU.Text = String.Empty;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Activo activo = activoServices.Read()[e.RowIndex];
                txtNombre.Text = activo.Nombre;
                txtValor.Text = activo.Valor.ToString();
                txtValorR.Text = activo.ValorResidual.ToString();
                txtVidaU.Text = activo.VidaUtil.ToString();

            }
            btnActualizar.Visible = true;
            btnEliminar.Visible = true;
            btnAceptar.Visible = false;
            cmbDepreciacion.Visible = true;
            richTextBox1.Visible = true;
            btnNuevo.Visible = true;
            cmbDepreciacion.SelectedIndex = -1;
            richTextBox1.Text = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbDepreciacion.Items.AddRange(Enum.GetValues(typeof(Depreciacion)).Cast<object>().ToArray());
            dataGridView1.DataSource = activoServices.Read();
            btnActualizar.Visible = false;
            btnEliminar.Visible = false;
            cmbDepreciacion.Visible = false;
            richTextBox1.Visible = false;
            btnNuevo.Visible = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            object n = dataGridView1.CurrentRow.Cells[0].Value;
            if (activoServices.Delete((Int32)n))
            {
                btnEliminar.Visible = false;
                btnActualizar.Visible = false;
                btnAceptar.Visible = true;
                cmbDepreciacion.Visible = false;
                richTextBox1.Visible = false;
                btnNuevo.Visible = false;
                limpiar();
                dataGridView1.DataSource = activoServices.Read();
            }
            else
            {
                MessageBox.Show("No se ha logrado borrar con éxito el activo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (verificar())
            {
                object n = dataGridView1.CurrentRow.Cells[0].Value;
                Activo activo = activoServices.GetById((int)n);
                activo.Nombre = txtNombre.Text;
                activo.Valor = Convert.ToDouble(txtValor.Text);
                activo.ValorResidual = Convert.ToDouble(txtValorR.Text);
                activo.VidaUtil = Convert.ToInt32(txtVidaU.Text);
                activoServices.Update(activo);
                btnEliminar.Visible = false;
                btnActualizar.Visible = false;
                btnAceptar.Visible = true;
                cmbDepreciacion.Visible = false;
                richTextBox1.Visible = false;
                btnNuevo.Visible = false;
                limpiar();
                dataGridView1.DataSource = activoServices.Read();
            }
            else
            {
                MessageBox.Show("Tienes que llenar todos los formularios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbDepreciacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (verificar())
            {
                Activo activo = new Activo()
                {
                    Nombre = txtNombre.Text,
                    Valor = double.Parse(txtValor.Text),
                    ValorResidual = double.Parse(txtValorR.Text),
                    VidaUtil = int.Parse(txtVidaU.Text)
                };
                richTextBox1.Text = String.Empty;
                double total = 0;
                IDepreciacionModel depreciacion = FactoryDeducciones.FactoryDepreciacion((Depreciacion)cmbDepreciacion.SelectedIndex);
                List<double> depreciaciones = depreciacion.Depreciacion(activo);
                for (int i = 0; i < depreciaciones.Count; i++)
                {
                    richTextBox1.Text += $"Depreciacion: {i + 1}: {depreciaciones[i]}\n";
                    total += depreciaciones[i];
                }
                richTextBox1.Text += $"Total: {total}\n";
                richTextBox1.Text += $"Valor residual: {activo.ValorResidual}";
            }
            else
            {
                MessageBox.Show("Tienes que llenar todos los formularios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiar();
            btnEliminar.Visible = false;
            btnActualizar.Visible = false;
            btnAceptar.Visible = true;
            cmbDepreciacion.Visible = false;
            richTextBox1.Visible = false;
            btnNuevo.Visible = false;

        }
        private Activo RetornarActivo(DataGridViewCellEventArgs e)
        {
            return activoServices.Read()[e.RowIndex];
        }

        private void BtnAddEmpleado_Click(object sender, EventArgs e)
        {
            if(this.PnlMain.Controls.Count > 0)
            {
                this.PnlMain.Controls.RemoveAt(0);
            }

            AgregarEmpleado emp = new AgregarEmpleado(this.PnlMain, this.PnActivo, empleadoServices);
            emp.TopLevel = false;
            emp.Dock = DockStyle.Fill;
            this.PnlMain.Controls.Add(emp);
            this.PnlMain.Tag = emp;
            emp.Show();
        }

        private void empleadoAutocomplete()
        {
            foreach(Empleado empleado in empleados)
            {
                colection.Add($"{empleado.Nombre} {empleado.Apellido}");
            }
            txtEmpleado.AutoCompleteCustomSource = colection;
        }

        public static void addEmpleado(Empleado empleado)
        {
            empleados.Add(empleado);
            colection.Add($"{empleado.Nombre} {empleado.Apellido}");
        }

        public static void updateEmpleado(Empleado empleado, string nombre)
        {
            empleados.RemoveAll(x => x.Id == empleado.Id);
            empleados.Add(empleado);
            colection.Remove(nombre);
            colection.Add($"{empleado.Nombre} {empleado.Apellido}");
        }

        public static void removeEmpleado(int id, string nombre)
        {
            empleados.RemoveAll(x => x.Id == id);
            colection.Remove(nombre);
        }

        public static List<Empleado> getEmpleados()
        {
            return empleados;
        }
    }
}
