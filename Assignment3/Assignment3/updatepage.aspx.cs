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
    public partial class WebForm2 : System.Web.UI.Page
    {
        SqlConnection cnMovie = new SqlConnection();
        DataTable movietable = new DataTable();
        DataTable directortable = new DataTable();
        DataTable genretable = new DataTable();

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
                LoadDirector();
                LoadGenre();
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
            deleteDD.DataSource = movietable;
            deleteDD.DataTextField = "MovieTitle";
            deleteDD.DataValueField = "MovieID";
            deleteDD.DataBind();

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
                cmd.Parameters["@id"].Value = deleteDD.SelectedValue;

                SqlDataReader dr = cmd.ExecuteReader();
                dtable.Load(dr);
                dr.Close();
            }

            movieDetail.DataSource = dtable;
            movieDetail.DataBind();

            cnMovie.Close();
        }

        protected void LoadDirector()
        {
            cnMovie.Open();
            directortable.Clear();
            string sql = "select * from Director";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                directortable.Load(dr);
                dr.Close();
            }
            cnMovie.Close();

            dList.DataSource = directortable;
            dList.DataTextField = "DirectorName";
            dList.DataValueField = "DirectorID";
            dList.DataBind();
        }

        protected void LoadGenre()
        {
            cnMovie.Open();
            genretable.Clear();
            string sql = "select * from Genre";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                genretable.Load(dr);
                dr.Close();
            }
            cnMovie.Close();

            gList.DataSource = genretable;
            gList.DataTextField = "GenreType";
            gList.DataValueField = "GenreID";
            gList.DataBind();
        }


        protected void add_Click(object sender, EventArgs e)
        {

            bool idInvalid = false;
            bool idFound = false;
            bool listsNotSelected = false;

            LoadTable();

            if (mid.Value == "" || mtitle.Value == "" || rdate.Value == "" || mid.Value.Length > 5)
            {
                idInvalid = true;
            }

            for (int crow = 0; crow < movietable.Rows.Count; crow++)
            {
                if (movietable.Rows[crow][0].ToString() == mid.Value)
                {
                    idFound = true;
                }
            }

            if (idInvalid)
            {
                Response.Write("<script>alert('Invalid Movie ID')</script>");
            }
            else if (idFound)
            {
                Response.Write("<script>alert('ID found')</script>");
            }
            else if (dList.SelectedValue == "" || gList.SelectedValue == "")
            {
                listsNotSelected = true;
                Response.Write("<script>alert('Select Director and Genre')</script>");
            }

            if (!idInvalid && !idFound && !listsNotSelected)
            {

                UpdateMovie();
                ResetAll();
            }
        }

        protected void UpdateMovie()
        {
            cnMovie.Open();
            string sql = "Insert into Movie (MovieID, MovieTitle, ReleaseDate, GenreID, DirectorID) Values (@mid, @mtitle, @rdate, @genre, @director)";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                //set up all parameters
                cmd.Parameters.Add("@mid", SqlDbType.VarChar);
                cmd.Parameters["@mid"].Value = mid.Value;
                cmd.Parameters.Add("@mtitle", SqlDbType.VarChar);
                cmd.Parameters["@mtitle"].Value = mtitle.Value;
                cmd.Parameters.Add("@rdate", SqlDbType.VarChar);
                cmd.Parameters["@rdate"].Value = rdate.Value;
                cmd.Parameters.Add("@genre", SqlDbType.VarChar);
                cmd.Parameters["@genre"].Value = gList.SelectedValue;
                cmd.Parameters.Add("@director", SqlDbType.VarChar);
                cmd.Parameters["@director"].Value = dList.SelectedValue;
                cmd.ExecuteNonQuery();
            }

            cnMovie.Close();
        }
        protected void ResetAll()
        {
            mid.Value = "";
            mtitle.Value = "";
            rdate.Value = "";
            dList.ClearSelection();
            gList.ClearSelection();

            LoadTable();
            LoadDirector();
            LoadGenre();
            LoadDDAndDetail();
        }



        protected void delete_Click(object sender, EventArgs e)
        {
            string mid, mtitle, rdate, genre, director;
            cnMovie.Open();
            DataTable maintable = new DataTable();
            string sql = "Select * from Movie where MovieID = @mid";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                cmd.Parameters.Add("@mid", SqlDbType.VarChar);
                cmd.Parameters["@mid"].Value = deleteDD.SelectedValue;
                SqlDataReader dr = cmd.ExecuteReader();
                maintable.Load(dr);

                mid = maintable.Rows[0][0].ToString();
                mtitle = maintable.Rows[0][1].ToString();
                rdate = maintable.Rows[0][2].ToString();
                genre = maintable.Rows[0][3].ToString();
                director = maintable.Rows[0][4].ToString();
            }
            cnMovie.Close();

           
            try
                {
                    deleteMovie();
                    ResetAll();
                }
                catch
                {
                    Response.Write("<script>alert('movie delete error');</script>");
                    UpdateMovie();
                }

            }
            
        protected void deleteMovie()
        {
            cnMovie.Open();
            string sql = "Delete from Movie where MovieID = @mid";
            using (SqlCommand cmd = new SqlCommand(sql, cnMovie))
            {
                //set up all parameters
                cmd.Parameters.Add("@mid", SqlDbType.VarChar);
                cmd.Parameters["@mid"].Value = deleteDD.SelectedValue;

                cmd.ExecuteNonQuery();
            }

            cnMovie.Close();
        }

        protected void deleteDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDetail();
        }
    }
}
