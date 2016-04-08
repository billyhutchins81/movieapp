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
    public partial class WebForm3 : System.Web.UI.Page
    {
        SqlConnection cnMovie = new SqlConnection();
        DataTable movietable = new DataTable();
        DataTable dtable = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            var cstr = System.Configuration.ConfigurationManager.ConnectionStrings["movieDBCN"];
            string strConn = cstr.ConnectionString;
            cnMovie = new SqlConnection(strConn);

            if (!IsPostBack)
            {
                LoadCurrentGenres();
                LoadTable();
                LoadDDAndDetail();
                LoadDetail();
            }
        }

        protected void LoadTable()
        {
            cnMovie.Open();
            string sql = "Select * From Director";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                dtable.Load(dr);
                dr.Close();
            }
            cnMovie.Close();
        }
        protected void LoadDDAndDetail()
        {
            directorDD.DataSource = dtable;
            directorDD.DataTextField = "DirectorName";
            directorDD.DataValueField = "DirectorID";
            directorDD.DataBind();

            LoadDetail();
        }

        protected void LoadDetail()
        {
            cnMovie.Open();
            DataTable dtable = new DataTable();
            string sql = "SELECT Movie.MovieTitle, Movie.ReleaseDate, Director.DirectorName, Genre.GenreType " +
                          "FROM Movie INNER JOIN Director ON Movie.DirectorID=Director.DirectorID " +
                          "INNER JOIN Genre ON Movie.GenreID=Genre.GenreID WHERE Director.DirectorID = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                cmd.Parameters.Add("@id", SqlDbType.VarChar);
                cmd.Parameters["@id"].Value = directorDD.SelectedValue;

                SqlDataReader dr = cmd.ExecuteReader();
                dtable.Load(dr);
                dr.Close();
            }

            directorGrid.DataSource = dtable;
            directorGrid.DataBind();

            cnMovie.Close();
        }
        protected void LoadCurrentGenres()
        {
            cnMovie.Open();

            string sql = "select Distinct GenreType from Genre";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                movietable.Load(dr);
                dr.Close();
            }
            cnMovie.Close();
            genreList.DataSource = movietable;
            genreList.DataValueField = "GenreType";
            genreList.DataTextField = "GenreType";
            genreList.DataBind();
        }

        protected void genrePick_Click(object sender, EventArgs e)
        {
            cnMovie.Open();
            DataTable dtable = new DataTable();
            string sql = "";
            
            
            sql = "SELECT Movie.MovieTitle, Movie.ReleaseDate, Director.DirectorName, Genre.GenreType " +
                          "FROM Movie INNER JOIN Director ON Movie.DirectorID=Director.DirectorID " +
                          "INNER JOIN Genre ON Movie.GenreID=Genre.GenreID WHERE Genre.GenreType = @gid";
            
           

            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                cmd.Parameters.Add("@gid", SqlDbType.VarChar);
                cmd.Parameters["@gid"].Value = genreList.SelectedValue;

                SqlDataReader dr = cmd.ExecuteReader();
                dtable.Load(dr);
                dr.Close();
            }

            genreGrid.DataSource = dtable;
            genreGrid.DataBind();

            cnMovie.Close();	
        }

        protected void directorPick()
        {
            cnMovie.Open();
            DataTable dtable = new DataTable();
            string sql = "";


            sql = "SELECT Movie.MovieTitle, Movie.ReleaseDate, Director.DirectorName, Genre.GenreType " +
                          "FROM Movie INNER JOIN Director ON Movie.DirectorID=Director.DirectorID " +
                          "INNER JOIN Genre ON Movie.GenreID=Genre.GenreID WHERE Director.DirectorID = @did";



            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                cmd.Parameters.Add("@did", SqlDbType.VarChar);
                cmd.Parameters["@did"].Value = directorDD.SelectedValue;

                SqlDataReader dr = cmd.ExecuteReader();
                dtable.Load(dr);
                dr.Close();
            }

            directorGrid.DataSource = dtable;
            directorGrid.DataBind();

            cnMovie.Close();	
        }

        protected void directorDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            directorPick();
        }

       
    }
}