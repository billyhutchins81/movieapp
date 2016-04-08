using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Assignment3
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection cnMovie = new SqlConnection();
        DataTable movietable = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
                var cstr = System.Configuration.ConfigurationManager.ConnectionStrings["movieDBCN"];
                string strConn = cstr.ConnectionString;
                cnMovie = new SqlConnection(strConn);

                //if i wanted to load grid on startup
                if (!IsPostBack)
                {
                    LoadTable();
                    LoadDDAndDetail();
                    LoadDetail();
                }
            }
        protected void LoadTable()
        {
            cnMovie.Open();
            string sql = "Select * From Movie";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                movietable.Load(dr);
                dr.Close();
            }
            cnMovie.Close();
        }
        protected void LoadDDAndDetail()
        {
            movieDD.DataSource = movietable;
            movieDD.DataTextField = "MovieTitle";
            movieDD.DataValueField = "MovieID";
            movieDD.DataBind();

            LoadDetail();
        }

        protected void LoadDetail()
        {
            cnMovie.Open();
            DataTable dtable = new DataTable();
            string sql = "SELECT Movie.MovieTitle, Movie.ReleaseDate, Director.DirectorName, Genre.GenreType " +
                          "FROM Movie INNER JOIN Director ON Movie.DirectorID=Director.DirectorID " + 
                          "INNER JOIN Genre ON Movie.GenreID=Genre.GenreID WHERE Movie.MovieID = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                cmd.Parameters.Add("@id", SqlDbType.VarChar);
                cmd.Parameters["@id"].Value = movieDD.SelectedValue;

                SqlDataReader dr = cmd.ExecuteReader();
                dtable.Load(dr);
                dr.Close();
            }

            movieDetail.DataSource = dtable;
            movieDetail.DataBind();

            cnMovie.Close();
        }

        protected void movieDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDetail();
        }

        }
    }
