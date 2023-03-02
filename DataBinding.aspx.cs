using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using Label = System.Web.UI.WebControls.Label;

namespace Proiect_GaitanaruTeodora_BDI
{
    public partial class Databinding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbEroareAn.Text = "";
            lbInsertValidare.Text = "";
            lbUpdateValidare.Text = "";
            lbError.Text = "";

            if (!IsPostBack)
            {
                PanelCarte.Visible = true;
                PanelMembru.Visible = false;
               
                // apel proceduri
                definireProceduraNrCartiPerAutor();
                definireFunctieSituatiiCarti();
                definireProceduraCartiMembru();
                definireProceduraIstoricImprumuturiAutor();
                definireProceduraNrCartiNrPagini();
                definireProceduraMembruCuNrMaximDePagini();

                GvToateImprumuturile.RowDataBound += new GridViewRowEventHandler(GvToateImprumuturile_RowDataBound);
                GvImprumut.ShowFooter = true;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ro-RO");

                // populare Dropdown
                populeazaDdAutorFiltrare();
                populeazaDdSelecteazaMembru();
                populareDdProcedura3();

            }
        }
        //public void resetV()
        //{
        //    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string tableName = "IMPRUMUT";
        //        string query = "DBCC CHECKIDENT('" + tableName + "', RESEED, 0)";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        connection.Open();
        //        command.ExecuteNonQuery();
        //        connection.Close();
        //    }
        //}


        // ==================== PANEL CARTE =====================
        protected void btnCarti_Click(object sender, EventArgs e)
        {
            if (PanelMembru.Visible)
            {
                PanelMembru.Visible = false;
                PanelCarte.Visible = true;
                DataTable dt = new DataTable();
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
                string query = "SELECT * FROM Carte";
                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(connString))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConn))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    GvCarte.DataSourceID = null;
                    GvCarte.DataSource = dt;
                    GvCarte.DataBind();
                } catch(Exception ex)
                {
                    lblExceptieCarte.Text = "A aparut o eroare: " + ex.Message;
                }
            }  
        }

        protected void GvCarte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int nr_disponibile = Convert.ToInt32(e.Row.Cells[7].Text);

                    if (nr_disponibile > 0)
                    {

                        e.Row.Cells[8].Text = "Disponibil";
                        e.Row.Cells[8].BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        e.Row.Cells[8].Text = "Indisponibil";
                        e.Row.Cells[8].BackColor = System.Drawing.Color.PaleVioletRed;
                    }

                    int id_autor;
                    if (int.TryParse(e.Row.Cells[3].Text, out id_autor))
                    {
                        string conn = ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
                        string select = "SELECT nume FROM Autor WHERE Id = @id_autor";

                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand command = new SqlCommand(select, connection);
                            command.Parameters.AddWithValue("@id_autor", id_autor);

                            connection.Open();
                            string nume_autor = command.ExecuteScalar().ToString();

                            e.Row.Cells[3].Text = nume_autor;
                        }
                    }
                } 
            } catch(Exception ex)
            {
                lblExceptieCarte.Text = "A aparut o eroare: " + ex.Message;
            }
        }

        public void populeazaDdAutorFiltrare()
        {
            string connection = ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString;

            try
            {
                SqlDataSource sqlDataSource = new SqlDataSource(connection, "SELECT Id, nume FROM Autor");
                sqlDataSource.DataBind();

                DdFiltrareAutor.DataSource = sqlDataSource;
                DdFiltrareAutor.DataTextField = "nume";
                DdFiltrareAutor.DataValueField = "Id";
                DdFiltrareAutor.DataBind();
            } catch(Exception ex)
            {
                lblExceptieCarte.Text = "A aparut o eroare: " + ex.Message;
            }

        }
        protected void DdFiltrareAutor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string autor = DdFiltrareAutor.SelectedValue;
            string disponibilitate = RadioButtonList1.SelectedValue;


            if (disponibilitate == "Toate")
            {
                GvCarteSortatDS.SelectCommand = "SELECT * FROM Carte WHERE id_autor = '" + autor + "'";
            }
            else if (disponibilitate == "Disponibile")
            {
                GvCarteSortatDS.SelectCommand = "SELECT * FROM Carte WHERE id_autor = '" + autor + "' AND nr_disponibile > 0";
            }
            else if (disponibilitate == "Indisponibile")
            {
                GvCarteSortatDS.SelectCommand = "SELECT * FROM Carte WHERE id_autor = '" + autor + "' AND nr_disponibile = 0";
            }

        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string autor = DdFiltrareAutor.SelectedValue;
            string disponibilitate = RadioButtonList1.SelectedValue;

            try
            {

                if (disponibilitate == "Toate")
                {
                    GvCarteSortatDS.SelectCommand = "SELECT * FROM Carte WHERE id_autor = '" + autor + "'";
                }
                else if (disponibilitate == "Disponibile")
                {
                    GvCarteSortatDS.SelectCommand = "SELECT * FROM Carte WHERE id_autor = '" + autor + "' AND nr_disponibile > 0";
                }
                else if (disponibilitate == "Indisponibile")
                {
                    GvCarteSortatDS.SelectCommand = "SELECT * FROM Carte WHERE id_autor = '" + autor + "' AND nr_disponibile = 0";
                }

                GvCarteSortat.DataBind();

            }catch(Exception ex)
            {
                lblExceptieCarte.Text = "A aparut o eroare: " + ex.Message;
            }
        }

        protected void GvCarteSortat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int nr_disponibile = Convert.ToInt32(e.Row.Cells[7].Text);

                    if (nr_disponibile > 0)
                    {

                        e.Row.Cells[8].Text = "Disponibil";
                        e.Row.Cells[8].BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        e.Row.Cells[8].Text = "Indisponibil";
                        e.Row.Cells[8].BackColor = System.Drawing.Color.PaleVioletRed;
                    }
                }
            } catch(Exception ex)
            {
                lblExceptieCarte.Text = "A aparut o eroare: " + ex.Message;
            }
        }

        protected void btnFiltreaza_Click(object sender, EventArgs e)
        {
            try
            {
                int an;
                if (!int.TryParse(tbAnAparitie.Text, out an))
                {
                    lbEroareAn.Text = "Introduceti un an valid";
                    lbEroareAn.ForeColor = Color.Red;
                    return;
                }
                if (an < 1000 || an > 2024)
                {
                    lbEroareAn.Text = "Introduceti un an între 1000 și 2024";
                    lbEroareAn.ForeColor = Color.Red;
                    return;
                }

                string select = "SELECT * FROM CARTE WHERE an_publicare < @an";

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                using (SqlCommand command = new SqlCommand(select, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@an", an);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        lbEroareAn.Text = "Nu exista carti publicate inainte de anul " + an;

                        lbEroareAn.ForeColor = Color.Red;
                        return;
                    }
                    lbEroareAn.Text = "";

                    GvAn.DataSource = reader;
                    GvAn.DataBind();
                }
            }catch(Exception ex)
            {
                lblExceptieCarte.Text = "A aparut o eroare: " + ex.Message;
            }
        }

        protected void GvAn_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = "Id";
                    e.Row.Cells[1].Text = "Titlu";
                    e.Row.Cells[2].Text = "Editura";
                    e.Row.Cells[3].Text = "Autor";
                    e.Row.Cells[4].Text = "An publicare";
                    e.Row.Cells[5].Text = "Nr. pagini";
                    e.Row.Cells[6].Text = "Nr. exemplare";
                    e.Row.Cells[7].Text = "Nr. disponibile";
                    e.Row.Cells[0].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    int id_autor;
                    if (int.TryParse(e.Row.Cells[3].Text, out id_autor))
                    {
                        string conn = ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
                        string selectM = "SELECT nume FROM Autor WHERE Id = @id_autor";

                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand command = new SqlCommand(selectM, connection);
                            command.Parameters.AddWithValue("@id_autor", id_autor);

                            connection.Open();
                            string autor = command.ExecuteScalar().ToString();

                            e.Row.Cells[3].Text = autor;
                        }
                    }
                    e.Row.Cells[0].Visible = false;
                }
            } catch(Exception ex)
            {
                lblExceptieCarte.Text = "A aparut o eroare: " + ex.Message;
            }
        }

        // ========== PARTEA DE STATISTICI ==========

        public void definireProceduraNrCartiPerAutor()
        {
            string sqlCreate = "CREATE PROCEDURE getNrCartiDisponibileAutori AS " +
                "SELECT a.nume AS Autor, SUM(nr_disponibile) AS Carti," +
                "CAST((SUM(nr_disponibile) * 100.0 / SUM(nr_exemplare)) AS DECIMAL(5, 2)) AS Procent " +
                "FROM carte c " +
                "JOIN autor a ON c.id_autor = a.Id " +
                "WHERE nr_disponibile > 0" +
                "GROUP BY a.nume ";

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                try
                {
                    sqlCommand.CommandText = "DROP PROCEDURE getNrCartiDisponibileAutori";
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                sqlCommand.CommandText = sqlCreate;
                sqlCommand.ExecuteNonQuery();

            }
        }

        // ========== PANEL MEMBRI ==========

        protected void btnMembri_Click(object sender, EventArgs e)
        {
            try
            {
                if (PanelCarte.Visible)
                {
                    PanelCarte.Visible = false;
                    PanelMembru.Visible= true;
                }

            }
            catch (Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }

        }
        protected void GvMembru_SelectedIndexChanged(object sender, EventArgs e)
        {
            GvImprumutCarte.Visible = false;
            try
            {
                GridViewRow selectedRow = GvMembru.SelectedRow;
                GvToateImprumuturile.Visible = false;
                lbIstoricImprumuturi.Visible = true;
                lbUpdateValidare.Text = "";
                lbInsertValidare.Text = "";

                GvImprumutCarte.DataSource = null;
                GvImprumutCarte.DataBind();
                if (GvImprumut.Rows.Count == 0)
                {
                    lbIstoricImprumuturi.Text = "Membrul: " + selectedRow.Cells[2].Text + " nu are imprumuturi!";
                }
                else
                {
                    lbIstoricImprumuturi.Text = "Puteti vizualiza istoricul imprumuturilor pentru membrul: " + selectedRow.Cells[2].Text + ". Pentru detalii despre carti selectati imprumutul.";

                }
                GvImprumutCarte.DataSource = null;
                GvImprumutCarte.DataBind();
            } catch(Exception ex)
            {
                lbExceptieMembru.Text = lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }
      
        protected void BtnToateImprumuturile_Click(object sender, EventArgs e)
        {
            GvImprumut.Visible = false;
            GvImprumutCarte.Visible = false;
            GvToateImprumuturile.Visible = true;

            lbIstoricImprumuturi.Visible = true;
            lbIstoricImprumuturi.Text = "Istoricul imprumuturilor:";
            try
            {
                DataTable table = new DataTable();
                string select = "SELECT * FROM IMPRUMUT";
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString()))
                {
                    using (SqlCommand command = new SqlCommand(select, connection))
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(table);
                        }
                    }
                }

                GvToateImprumuturile.DataSource = table;
                GvToateImprumuturile.DataSourceID = null;
                GvToateImprumuturile.DataBind();
            }
            catch (Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }

        }

        protected void GvToateImprumuturile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = "Nr. imprumutului";
                    e.Row.Cells[1].Text = "Membru";
                    e.Row.Cells[2].Text = "Data imprumut";
                    e.Row.Cells[3].Text = "Data scadenta";
                    e.Row.Cells[4].Text = "Data returnare";
                    e.Row.Cells[0].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DateTime? dataImprumut = (e.Row.DataItem as DataRowView)["data_imprumut"] as DateTime?;
                    DateTime? dataScadenta = (e.Row.DataItem as DataRowView)["data_scadenta"] as DateTime?;
                    DateTime? dataReturnare = (e.Row.DataItem as DataRowView)["data_returnare"] as DateTime?;

                    if (dataReturnare == null)
                    {
                        e.Row.BackColor = System.Drawing.Color.Pink;
                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.Color.LightGreen;
                    }

                    int id_membru;
                    if (int.TryParse(e.Row.Cells[1].Text, out id_membru))
                    {
                        string conn = ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
                        string selectM = "SELECT nume FROM Membru WHERE Id = @id_membru";

                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand command = new SqlCommand(selectM, connection);
                            command.Parameters.AddWithValue("@id_membru", id_membru);

                            connection.Open();
                            string membru = command.ExecuteScalar().ToString();

                            e.Row.Cells[1].Text = membru;
                        }
                    }
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[2].Text = dataImprumut?.ToString("d");
                    e.Row.Cells[3].Text = dataScadenta?.ToString("d");
                    e.Row.Cells[4].Text = dataReturnare?.ToString("d");
                }
            } catch (Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }

        }
        protected void GvImprumut_SelectedIndexChanged1(object sender, EventArgs e)
        {
            GvImprumutCarte.Visible = true;
        }

        protected void btnInserare_Click(object sender, EventArgs e)
        {
            lbUpdateValidare.Visible = false;
            lbInsertValidare.Visible = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Imprumut(id_membru, data_imprumut, data_scadenta) VALUES (@id_membru, @data_imprumut, @data_scadenta)", conn);

                    DropDownList id_membru = (DropDownList)GvImprumut.FooterRow.FindControl("DdMembruIns");
                    TextBox data_imprumut = (TextBox)GvImprumut.FooterRow.FindControl("txtDataIInserare");
                    if (string.IsNullOrEmpty(data_imprumut.Text))
                    {
                        lbInsertValidare.Text = "Data imprumut trebuie completata!";
                        lbInsertValidare.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        lbInsertValidare.Text = "";
                    }
                    DateTime data_scadenta = DateTime.Parse(data_imprumut.Text).AddDays(21);

                    cmd.Parameters.AddWithValue("@id_membru", id_membru.SelectedValue);
                    cmd.Parameters.AddWithValue("@data_imprumut", data_imprumut.Text);
                    cmd.Parameters.AddWithValue("@data_scadenta", data_scadenta);

                    cmd.ExecuteNonQuery();


                }
                GvImprumut.DataBind();
            } catch(Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }
        protected void GvImprumut_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lbUpdateValidare.Text = "";
            lbInsertValidare.Text = "";
        }

        protected void GvImprumut_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lbInsertValidare.Visible = false;
            lbUpdateValidare.Visible = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Imprumut SET id_membru=@id_membru, data_imprumut=@data_imprumut, data_scadenta=@data_scadenta, data_returnare=@data_returnare WHERE Id=@id_imprumut", conn);
                    cmd.Parameters.AddWithValue("@id_imprumut", GvImprumut.DataKeys[e.RowIndex].Value);
                    DropDownList id_membru = (DropDownList)GvImprumut.Rows[e.RowIndex].FindControl("DdMembruEditare");
                    Debug.WriteLine(id_membru.SelectedValue);
                    TextBox data_r = (TextBox)GvImprumut.Rows[e.RowIndex].FindControl("TbDataREditare");
                    TextBox data_i = (TextBox)GvImprumut.Rows[e.RowIndex].FindControl("TextBox2");
                    TextBox data_s = (TextBox)GvImprumut.Rows[e.RowIndex].FindControl("TextBox3");

                    if (string.IsNullOrEmpty(data_r.Text))
                    {
                        lbUpdateValidare.Text = "Sunteti in modul editare! Va rog sa completati data returnarii!";
                        lbUpdateValidare.ForeColor = Color.Red;
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        lbUpdateValidare.Text = "";
                    }

                    DateTime data_returnare = DateTime.Parse(data_r.Text);
                    DateTime data_imprumut = DateTime.Parse(data_i.Text);
                    DateTime data_scadenta = DateTime.Parse(data_s.Text);

                    if (data_returnare < data_imprumut || data_returnare > data_scadenta )
                    {
                        lbUpdateValidare.Text = "Data de returnare trebuie sa fie intre data de imprumut si data scadentei!";
                        lbUpdateValidare.ForeColor = Color.Red;
                        e.Cancel = true;
                        return;
                    }

                    SqlCommand cmdUpdate = new SqlCommand("UPDATE CARTE SET nr_disponibile = nr_disponibile + 1 WHERE Id IN (SELECT id_carte FROM IMPRUMUT_CARTE WHERE id_imprumut = @id_imprumut)", conn);
                    cmdUpdate.Parameters.AddWithValue("@id_imprumut", GvImprumut.DataKeys[e.RowIndex].Value);
                    cmdUpdate.ExecuteNonQuery();

                    cmd.Parameters.AddWithValue("@id_membru", id_membru.SelectedValue);
                    cmd.Parameters.AddWithValue("@data_imprumut", data_imprumut);
                    cmd.Parameters.AddWithValue("@data_scadenta", data_scadenta);
                    cmd.Parameters.AddWithValue("@data_returnare", data_returnare);

                    cmd.ExecuteNonQuery();
                }
            } catch(Exception ex)
            {
                lbExceptieMembru.Text = lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }

        protected void btnAdaugaCarte_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString);
            conn.Open();
            lbError.Text = "";
            SqlCommand cmdSelect = new SqlCommand("SELECT nr_disponibile FROM CARTE WHERE Id=@carte", conn);
            int id_imprumut_selectat = Convert.ToInt32(GvImprumut.SelectedDataKey.Value);
            DropDownList id_carte = (DropDownList)GvImprumutCarte.FooterRow.FindControl("DdAdaugaCarte");

            try
            {
                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM IMPRUMUT_CARTE WHERE id_imprumut=@imprumut", conn);
                cmdCount.Parameters.AddWithValue("@imprumut", id_imprumut_selectat);
                int nr_carti = (int)cmdCount.ExecuteScalar();
                if (nr_carti < 3)
                {

                    SqlCommand cmdConditie = new SqlCommand("SELECT COUNT(*) FROM IMPRUMUT_CARTE WHERE id_imprumut=@imprumut AND id_carte=@carte", conn);
                    cmdConditie.Parameters.AddWithValue("@imprumut", id_imprumut_selectat);
                    cmdConditie.Parameters.AddWithValue("@carte", id_carte.SelectedValue);
                    int count = (int)cmdConditie.ExecuteScalar();

                    if (count == 0)
                    {
                        cmdSelect.Parameters.AddWithValue("@carte", id_carte.SelectedValue);
                        int nr_disponibile = (int)cmdSelect.ExecuteScalar();
                        Debug.WriteLine(nr_disponibile);


                        if (nr_disponibile > 0)
                        {
                            // Inserare in tabela imprumut_carte
                            SqlCommand cmdInsert = new SqlCommand("INSERT INTO IMPRUMUT_CARTE(id_imprumut, id_carte)" +
                            " VALUES (@imprumut, @carte)", conn);
                            cmdInsert.Parameters.AddWithValue("@imprumut", System.Data.SqlDbType.Int);
                            cmdInsert.Parameters.AddWithValue("@carte", System.Data.SqlDbType.Int);

                            cmdInsert.Parameters["@imprumut"].Value = id_imprumut_selectat;
                            cmdInsert.Parameters["@carte"].Value = id_carte.SelectedValue;

                            cmdInsert.ExecuteNonQuery();

                            // Actualizare nr_exemplare
                            lbError.Text = "";
                            SqlCommand cmdUpdate = new SqlCommand("UPDATE CARTE SET nr_disponibile=nr_disponibile-1 WHERE Id=@carte", conn);
                            cmdUpdate.Parameters.AddWithValue("@carte", id_carte.SelectedValue);
                            cmdUpdate.ExecuteNonQuery();

                            GvImprumutCarte.DataBind();
                        }
                        else
                        {
                            lbError.Text = "Cartea nu poate fi imprumutata deoarece nu există exemplare disponibile.";
                            lbError.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lbError.Text = "Nu puteti imprumuta aceeasi carte de 2 ori.";
                        lbError.ForeColor = System.Drawing.Color.Red;

                    }
                }
                else
                {
                    lbError.Text = "Nr maxim de carti pe imprumut a fost atins!";
                    lbError.ForeColor = System.Drawing.Color.Red;
                }
            } catch(Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }

        }
       
        protected void GvImprumutCarte_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView gridView = (GridView)sender;

                Button btnAdaugaCarte = e.Row.FindControl("btnAdaugaCarteHeader") as Button;
                DropDownList ddAutorInserare = e.Row.FindControl("DdAutorInserareHeader") as DropDownList;
                DropDownList ddAdaugaCarte = e.Row.FindControl("DdAdaugaCarteHeader") as DropDownList;


                if (btnAdaugaCarte != null)
                {
                    btnAdaugaCarte.Visible = (gridView.Rows.Count == 0);
                }

                if (ddAutorInserare != null)
                {
                    ddAutorInserare.Visible = (gridView.Rows.Count == 0);
                }

                if (ddAdaugaCarte != null)
                {
                    ddAdaugaCarte.Visible = (gridView.Rows.Count == 0);
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == 0)
            {
                GridView gridView = (GridView)sender;
                Button btnAdaugaCarte = gridView.HeaderRow.FindControl("btnAdaugaCarteHeader") as Button;
                DropDownList ddAutorInserare = gridView.HeaderRow.FindControl("DdAutorInserareHeader") as DropDownList;
                DropDownList ddAdaugaCarte = gridView.HeaderRow.FindControl("DdAdaugaCarteHeader") as DropDownList;

                if (btnAdaugaCarte != null)
                {
                    btnAdaugaCarte.Visible = false;
                }

                if (ddAutorInserare != null)
                {
                    ddAutorInserare.Visible = false;
                }

                if (ddAdaugaCarte != null)
                {
                    ddAdaugaCarte.Visible = false;
                }


            }
        }

        protected void btnAdaugaCarteHeader_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString);
                conn.Open();

                SqlCommand cmdSelect = new SqlCommand("SELECT nr_disponibile FROM CARTE WHERE Id=@carte", conn);
                int id_imprumut_selectat = Convert.ToInt32(GvImprumut.SelectedDataKey.Value);
                DropDownList id_carte = (DropDownList)GvImprumutCarte.HeaderRow.FindControl("DdAdaugaCarteHeader");

                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM IMPRUMUT_CARTE WHERE id_imprumut=@imprumut", conn);
                cmdCount.Parameters.AddWithValue("@imprumut", id_imprumut_selectat);
                int nr_carti = (int)cmdCount.ExecuteScalar();
                if (nr_carti < 3)
                {

                    SqlCommand cmdConditie = new SqlCommand("SELECT COUNT(*) FROM IMPRUMUT_CARTE WHERE id_imprumut=@imprumut AND id_carte=@carte", conn);
                    cmdConditie.Parameters.AddWithValue("@imprumut", id_imprumut_selectat);
                    cmdConditie.Parameters.AddWithValue("@carte", id_carte.SelectedValue);
                    int count = (int)cmdConditie.ExecuteScalar();

                    if (count == 0)
                    {
                        cmdSelect.Parameters.AddWithValue("@carte", id_carte.SelectedValue);
                        int nr_disponibile = (int)cmdSelect.ExecuteScalar();
                        Debug.WriteLine(nr_disponibile);


                        if (nr_disponibile > 0)
                        {
                            // Inserare in tabela imprumut_carte
                            SqlCommand cmdInsert = new SqlCommand("INSERT INTO IMPRUMUT_CARTE(id_imprumut, id_carte)" +
                            " VALUES (@imprumut, @carte)", conn);
                            cmdInsert.Parameters.AddWithValue("@imprumut", System.Data.SqlDbType.Int);
                            cmdInsert.Parameters.AddWithValue("@carte", System.Data.SqlDbType.Int);

                            cmdInsert.Parameters["@imprumut"].Value = id_imprumut_selectat;
                            cmdInsert.Parameters["@carte"].Value = id_carte.SelectedValue;

                            cmdInsert.ExecuteNonQuery();

                            // Actualizare nr_disponibile
                            SqlCommand cmdUpdate = new SqlCommand("UPDATE CARTE SET nr_disponibile=nr_disponibile-1 WHERE Id=@carte", conn);
                            cmdUpdate.Parameters.AddWithValue("@carte", id_carte.SelectedValue);
                            cmdUpdate.ExecuteNonQuery();

                            GvImprumutCarte.DataBind();
                        }
                        else
                        {
                            lbError.Text = "Cartea nu poate fi imprumutata deoarece nu există exemplare disponibile.";
                            lbError.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lbError.Text = "Nu puteti imprumuta aceeasi carte de 2 ori.";
                        lbError.ForeColor = System.Drawing.Color.Red;

                    }
                }
                else
                {
                    lbError.Text = "Nr maxim de carti pe imprumut a fost atins!";
                    lbError.ForeColor = System.Drawing.Color.Red;
                }
            } catch(Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }

        protected void GvImprumutCarte_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
            TextBox txtIdCarte = (TextBox)GvImprumutCarte.Rows[e.RowIndex].FindControl("TextBox1");
            int id_carte = Convert.ToInt32(txtIdCarte.Text);
            Debug.WriteLine(id_carte);
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateCommand = "UPDATE CARTE SET nr_disponibile = nr_disponibile + 1 WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(updateCommand, connection))
                    {
                        lbError.Text = "";
                        command.Parameters.AddWithValue("@Id", id_carte);
                        command.ExecuteNonQuery();
                    }
                }
            }catch(Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }
        protected string GetNumeMembru(object id_membru)
        {
            string nume_membru = "";
            if (id_membru != DBNull.Value && id_membru != null)
            {
                int id = Convert.ToInt32(id_membru);
                string query = "SELECT nume FROM MEMBRU WHERE Id = @Id";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            nume_membru = result.ToString();
                        }
                    }
                }
            }
            return nume_membru;
        }

        protected string GetTitluCarte(object id_carte)
        {
            string titlu_carte = "";
            if (id_carte != DBNull.Value && id_carte != null)
            {
                int id = Convert.ToInt32(id_carte);
                string query = "SELECT titlu FROM CARTE WHERE Id = @Id";
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            titlu_carte = result.ToString();
                        }
                    }
                }
            }
            return titlu_carte;
        }

        // ==================== PROCEDURI SI FUNCTII ====================
        // FUNCTIE 1
        public void definireFunctieSituatiiCarti()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("DROP FUNCTION IF EXISTS getSituatiiCarti", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Definire functie
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("CREATE FUNCTION getSituatiiCarti() RETURNS NVARCHAR(50) AS BEGIN DECLARE @exemplareTotal INT, @exemplareImprumutate INT, @exemplareDisponibile INT " +
                        "SELECT @exemplareTotal = SUM(nr_exemplare)," +
                        "@exemplareImprumutate = SUM(nr_exemplare) - SUM(nr_disponibile)," +
                        "@exemplareDisponibile = SUM(nr_disponibile) " +
                        "FROM carte " +
                        "RETURN CAST(@exemplareTotal AS NVARCHAR(10)) + ',' + CAST(@exemplareImprumutate AS NVARCHAR(10)) + ',' + CAST(@exemplareDisponibile AS NVARCHAR(10)) " +
                        "END", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch(Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }
        protected void btnFunctie1_Click(object sender, EventArgs e)
        {
            lbFunctie1.Visible = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT dbo.getSituatiiCarti()", connection))
                    {

                        string situatiiCarti = cmd.ExecuteScalar().ToString();
                        string[] situatiiCartiArr = situatiiCarti.Split(',');
                        lbFunctie1.Text = string.Format("⚫ La data actuala {0}, din totalul de {1} carti ale bibliotecii, {2} sunt imprumutate si {3} sunt disponibile.", DateTime.Now.ToString("dd.MM.yyyy"), situatiiCartiArr[0], situatiiCartiArr[1], situatiiCartiArr[2]);
                    }
                }
            } catch(Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }

        // PROCEDURA 2

        public void definireProceduraIstoricImprumuturiAutor()
        {
            string sqlCreate = "CREATE PROCEDURE getIstoricImprumuturiAutor AS " +
                "SELECT a.nume AS Autor, " +
                "COUNT(*) AS Imprumuturi, " +
                "CAST((COUNT(*) * 100.0 / (SELECT COUNT(*) FROM imprumut_carte)) AS DECIMAL(5, 2)) AS Procent " +
                "FROM autor a JOIN carte c ON a.Id = c.id_autor " +
                "JOIN imprumut_carte ic ON c.Id = ic.id_carte GROUP BY a.nume";

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        sqlCommand.CommandText = "DROP PROCEDURE getIstoricImprumuturiAutor";
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                    sqlCommand.CommandText = sqlCreate;
                    sqlCommand.ExecuteNonQuery();
                }
            } catch(Exception ex) {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
        }

        protected void btnProcedura2_Click(object sender, EventArgs e)
        {
            GvProcedura2.Visible = true;
            lbProcedura2.Visible = true;

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("getIstoricImprumuturiAutor", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                GvProcedura2.DataSource = dt;
                GvProcedura2.DataBind();
            }
        }
        // PROCEDURA 3
        protected void btnProcedura3_Click(object sender, EventArgs e)
        {
            DdProcedura3.Visible = true;
            lbProcedura3Carti.Visible = true;
            lbProcedura3Pagini.Visible = true;
            tbProcedura3Carti.Visible = true;
            tbProcedura3Pagini.Visible = true;
            Label10.Visible = true;
            btnProcedura4.Visible = true;
            lbProc3.Visible = true;
        }

        public void definireProceduraNrCartiNrPagini()
        {
            string sqlCreate = "CREATE PROCEDURE getNrCartiNrPagini (@id_membru INT, @nr_carti INT OUTPUT, @nr_pagini INT OUTPUT) AS " +
                                "SELECT @nr_carti = COUNT(ic.id_carte), @nr_pagini = SUM(c.nr_pagini) " +
                                "FROM IMPRUMUT i INNER JOIN IMPRUMUT_CARTE ic ON i.Id = ic.id_imprumut " +
                                "INNER JOIN CARTE c on ic.id_carte = c.Id " +
                                "WHERE i.id_membru = @id_membru ";


            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                try
                {
                    sqlCommand.CommandText = "DROP PROCEDURE getNrCartiNrPagini";
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                sqlCommand.CommandText = sqlCreate;
                sqlCommand.ExecuteNonQuery();
            }
        }
        public void populareDdProcedura3()
        {
            string connection = ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString;
            SqlConnection con = new SqlConnection(connection);
            try
            {
                DataTable dt = new DataTable();
                string query = "Select * from  MEMBRU;";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                con.Open();
                da.Fill(dt);

                DdProcedura3.DataSource = dt;
                DdProcedura3.DataTextField = "nume";
                DdProcedura3.DataValueField = "Id";
                DdProcedura3.DataBind();
                DdProcedura3.Items.Insert(0, new ListItem("Selecteaza membrul", "0"));
            }
            catch
            {
                lblError.Text = lblError.Text = "A apărut o eroare";
            }
            finally
            {
                con.Close();
            }
        }
        protected void DdProcedura3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_membru = int.Parse(DdProcedura3.SelectedValue);

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("getNrCartiNrPagini", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_membru", id_membru);

                        SqlParameter paramNrCarti = new SqlParameter("@nr_carti", SqlDbType.Int);
                        paramNrCarti.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramNrCarti);

                        SqlParameter paramNrPagini = new SqlParameter("@nr_pagini", SqlDbType.Int);
                        paramNrPagini.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramNrPagini);

                        cmd.ExecuteNonQuery();

                        int nrCarti = Convert.ToInt32(paramNrCarti.Value);
                        Debug.WriteLine(nrCarti);
                        int nrPagini = Convert.ToInt32(paramNrPagini.Value);
                        Debug.WriteLine(nrPagini);

                        tbProcedura3Carti.Text = nrCarti.ToString();
                        tbProcedura3Pagini.Text = nrPagini.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "A apărut o eroare: Membrul nu are imprumuturi";
            }
        }

        // PROCEDURA 4
        public void definireProceduraMembruCuNrMaximDePagini()
        {
            string sqlCreate = "CREATE PROCEDURE getCelMaiMultePagini AS " +
                        "SELECT TOP 1 m.nume, COUNT(ic.id_carte), SUM(c.nr_pagini) " +
                        "FROM MEMBRU m " +
                        "INNER JOIN IMPRUMUT i ON m.Id = i.id_membru " +
                        "INNER JOIN IMPRUMUT_CARTE ic ON i.Id = ic.id_imprumut " +
                        "INNER JOIN CARTE c ON ic.id_carte = c.Id " +
                        "GROUP BY m.nume " +
                        "ORDER BY SUM(c.nr_pagini) DESC ";

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                try
                {
                    sqlCommand.CommandText = "DROP PROCEDURE getCelMaiMultePagini";
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                sqlCommand.CommandText = sqlCreate;
                sqlCommand.ExecuteNonQuery();
            }
        }
        protected void btnProcedura4_Click(object sender, EventArgs e)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("getCelMaiMultePagini", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string numeMembru = reader.GetString(0);
                    int nrCarti = reader.GetInt32(1);
                    Debug.WriteLine(nrCarti);
                    int nrPagini = reader.GetInt32(2);
                    Debug.WriteLine(nrPagini);
                    lblProcedura4.Text = $"Membrul cu cele mai multe pagini citite este {numeMembru}, cu un numar total de {nrCarti} carti si {nrPagini} de pagini citite.";
                    lblProcedura4.Visible = true;
                }
                else
                {
                    lblProcedura4.Text = "Nu existA membri cu imprumuturi in baza de date.";
                }
            }
        }

        // PROCEDURA 
        protected void btnProcedura5_Click(object sender, EventArgs e)
        {
            lblProcedura5.Visible = true;
            ddProcedura5.Visible = true;
            txtProcedura5.Visible = true;

        }
        public void populeazaDdSelecteazaMembru()
        {
            string connection = ConfigurationManager.ConnectionStrings["bibliotecaCS"].ConnectionString;
            SqlConnection con = new SqlConnection(connection);
            try
            {
                DataTable dt = new DataTable();
                string query = "Select * from  MEMBRU;";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                con.Open();
                da.Fill(dt);

                ddProcedura5.DataSource = dt;
                ddProcedura5.DataTextField = "nume";
                ddProcedura5.DataValueField = "Id";
                ddProcedura5.DataBind();
                ddProcedura5.Items.Insert(0, new ListItem("Selecteaza membrul", "0"));

            }
            catch(Exception ex)
            {
                lbExceptieMembru.Text = "A aparut o eroare!" + ex.Message;
                lbExceptieMembru.ForeColor = Color.Red;
            }
            finally
            {
                con.Close();
            }
        }
        public void definireProceduraCartiMembru()
        {
            string sqlCreate = "CREATE PROCEDURE getImprumuturiCartiMembru (@id_membru INT) AS " +
                 "SELECT i.Id, c.titlu, i.data_imprumut " +
                 "FROM IMPRUMUT_CARTE ic " +
                 "JOIN IMPRUMUT i ON i.Id = ic.id_imprumut " +
                 "JOIN CARTE c ON c.Id = ic.id_carte " +
                 "WHERE i.id_membru = @id_membru";

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = System.Data.CommandType.Text;
                try
                {
                    sqlCommand.CommandText = "DROP PROCEDURE getImprumuturiCartiMembru";
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    lbError.Text = ex.Message;
                }
                sqlCommand.CommandText = sqlCreate;
                sqlCommand.ExecuteNonQuery();
            }
        }
        protected void DdSelecteazaMembru_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idMembru = int.Parse(ddProcedura5.SelectedValue);
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("getImprumuturiCartiMembru", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@id_membru", System.Data.SqlDbType.Int).Value = idMembru;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            string results = "";
                            while (reader.Read())
                            {
                                results += "Titlu carte: " + reader["titlu"].ToString() + "\r\n";
                                results += "Data imprumut: " + ((DateTime)reader["data_imprumut"]).ToString("dd/MM/yyyy") + "\r\n";
                                results += "\r\n";
                            }
                            txtProcedura5.Text = results;
                        }
                    }
                }

                lbError.Text = "";
            }
            catch (Exception ex)
            {
                lbError.Text = ex.Message;
            }

        }

        // ==================== GRAFICE ====================
        protected void btnGrafic1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                string select = "SELECT MONTH(data_imprumut) AS Luna," +
                                " COUNT(*) AS Imprumuturi FROM IMPRUMUT" +
                                " WHERE YEAR(data_imprumut) = 2023" +
                                " GROUP BY MONTH(data_imprumut)";


                using (SqlDataAdapter adapter = new SqlDataAdapter(select, sqlConnection))
                {
                    adapter.Fill(dt);
                    Cache["imprumuturi_luna"] = dt;
                    Response.Redirect("Grafic.aspx?type=" + "1");
                }
            }
        }

        protected void btnGrafic2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["bibliotecaCS"].ToString();
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("getIstoricImprumuturiAutor", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                    {
                        adapter.Fill(dt);
                        Cache["istoric_autori"] = dt;
                        Response.Redirect("Grafic.aspx?type=" + "2");
                    }
                }
            }
        }

    }
 }


