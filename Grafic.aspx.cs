using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZedGraph;

namespace Proiect_GaitanaruTeodora_BDI
{
    public partial class Grafic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ZedGraphWeb1.RenderGraph += this.ZedGraphWeb1_RenderGraph;
        }

        private void ZedGraphWeb1_RenderGraph(ZedGraph.Web.ZedGraphWeb webObject, System.Drawing.Graphics g, ZedGraph.MasterPane pane)
        {
            try
            {
                switch (Request.QueryString["type"])
                {
                    case "1":
                        {
                            imprumuturi_luna(pane);
                            break;
                        }
                    case "2":
                        {
                            istoric_autori(pane);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {

            }
            
        }
        private void imprumuturi_luna(MasterPane pane)
        {
            DataTable data = (DataTable)Cache["imprumuturi_luna"];
            GraphPane mpane = pane[0];
            mpane.Title.Text = "Evolutia imprumuturilor pe lunile anului 2023.";
            mpane.XAxis.Title.Text = "Luna";
            mpane.YAxis.Title.Text = "Nr. imprumuturi";
           
            PointPairList lista_puncte = new PointPairList();
            for (int i = 1; i <= 12; i++)
            {
                int nr_imprumuturi = 0;
                foreach (DataRow row in data.Rows)
                {
                    int luna = Convert.ToInt32(row["Luna"]);
                    int nr = Convert.ToInt32(row["Imprumuturi"]);
                    if (luna == i)
                    {
                        nr_imprumuturi = nr;
                        break;
                    }
                }
                lista_puncte.Add(i, nr_imprumuturi);
            }

            LineItem curve = mpane.AddCurve("Nr.imprumuturi", lista_puncte, Color.DarkBlue, SymbolType.Circle);
            curve.Line.IsSmooth = true;
            curve.Line.SmoothTension = 0.5F;
            curve.Line.Width = 2;

            curve.Symbol.Fill = new Fill(Color.White);
            curve.Symbol.Size = 10;

            string[] luni = { "Ian", "Feb", "Mar", "Apr", "Mai", "Iun", "Iul", "Aug", "Sep", "Oct", "Noi", "Dec" };
            mpane.XAxis.Scale.TextLabels = luni;
            mpane.XAxis.Type = AxisType.Text;
        }

            private void istoric_autori(MasterPane pane)
        {
            DataTable data = (DataTable)Cache["istoric_autori"];
            GraphPane mpane = pane[0];
            mpane.Title.Text = "Istoricul imprumuturilor pe autor";
            mpane.Title.FontSpec.Size = 16f;
            mpane.Legend.IsVisible = true;
            mpane.Legend.Position = LegendPos.BottomCenter;
            mpane.Legend.FontSpec.Size = 12f;
            mpane.Legend.FontSpec.Border.IsVisible = false;
            mpane.Legend.FontSpec.Fill = new Fill(Color.WhiteSmoke);
            mpane.Chart.Fill = new Fill(Color.WhiteSmoke); ;
            

            ColorSymbolRotator rotator = new ColorSymbolRotator();
            Double total = 0;
            foreach (DataRow r in data.Rows)
            {
                total += Convert.ToDouble(r[1]);

            }
            if (Request.QueryString["type"] != null)
            {

                foreach (DataRow r in data.Rows)
                {
                    double value = Convert.ToDouble(r["Imprumuturi"]);
                    string label = r["Autor"].ToString();
                    double percent = Convert.ToDouble(r["Procent"]) / 100.0;
                    PieItem slice = mpane.AddPieSlice(value, ColorSymbolRotator.StaticNextColor, Color.White, 45f, 0, $"{label}: {value} ({percent.ToString("P")})");
                    slice.LabelDetail.FontSpec.Size = 12f;
                    slice.LabelDetail.FontSpec.IsBold = false;
                    slice.LabelDetail.FontSpec.Border.IsVisible = false;
                    slice.LabelDetail.FontSpec.Fill = new Fill(Color.WhiteSmoke);   
                }
            }
        }

        protected void btnInapoi_Click(object sender, EventArgs e)
        {
            Response.Redirect("Databinding.aspx");
        }

    }
}