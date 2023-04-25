using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace THA_W8_BENITO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable dtteam = new DataTable();
        DataTable dtplayer = new DataTable();
        DataTable dtselected = new DataTable();
        DataTable dtmusolo = new DataTable();

        DataTable dttuanrumah = new DataTable();    
        DataTable dttamu = new DataTable();

        DataTable dtcadangan = new DataTable();

        string query = "";
        MySqlConnection connection = new MySqlConnection();     
        MySqlDataAdapter adapter = new MySqlDataAdapter();  
        MySqlCommand command = new MySqlCommand();
        string localhost = "server=localhost;uid=root;pwd=benitopriyasha2004;database=premier_league;";
        

        private void playerDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dtteam.Clear();
            dtplayer.Clear();
            dtselected.Clear();

            query = "select team_name,team_id from team;";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter (command);
            adapter.Fill(dtteam);
            comboBox1.DataSource = dtteam;
            comboBox1.ValueMember = "team_id";
            comboBox1.DisplayMember = "team_name";
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(localhost);
      
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dtselected.Clear();
            dtplayer.Clear();
            query = $"select p.player_name from player p, team t WHERE p.team_id = t.team_id AND t.team_id = '{comboBox1.SelectedValue.ToString()}';";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dtselected);
            comboBox2.DataSource = dtselected;
            comboBox2.ValueMember = "player_name";
            comboBox2.DisplayMember = "player_name";

        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dtplayer.Clear();
            dtmusolo.Clear();   
            query = $"select p.player_name,t.team_name,n.nation,if(p.playing_pos = 'F','Forward',if(p.playing_pos = 'M','Midfielder',if (p.playing_pos = 'D','Defender','Goalkeeper'))),p.team_number from player p, team t , nationality n where p.team_id = t.team_id and n.nationality_id = p.nationality_id and p.player_name ='{comboBox2.SelectedValue.ToString()}';";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dtplayer);

            query = $"select ifnull(sum(case when d.`type`='CY'then 1 else 0 end),0),ifnull(sum(case when d.`type`='CR'then 1 else 0 end),0),ifnull(sum(case when d.`type`='GO'then 1 else 0 end),0),ifnull(sum(case when d.`type`='GW'then 1 else 0 end),0),ifnull(sum(case when d.`type`='GP'then 1 else 0 end),0),ifnull(sum(case when d.`type`='PM'then 1 else 0 end),0) from dmatch d , player p where d.player_id = p.player_id and p.player_name ='{comboBox2.SelectedValue.ToString()}';";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dtmusolo);

            label1.Text = "player name :";
            label2.Text = "team name :";
            label3.Text = "position :";
            label4.Text = "squad number :";
            label5.Text = "yellow card :";
            label6.Text = "red card :";
            label7.Text = "goal :";
            label8.Text = "own goal :";
            label9.Text = "goal penalty :";
            label10.Text = "penalty missed :";
            label21.Text = "nationality:";
            label11.Text = dtplayer.Rows[0][0].ToString();
            label12.Text = dtplayer.Rows[0][1].ToString();
            label13.Text = dtplayer.Rows[0][2].ToString();
            label14.Text = dtplayer.Rows[0][3].ToString();
            label15.Text = dtplayer.Rows[0][4].ToString();
            label16.Text = dtmusolo.Rows[0][0].ToString();
            label17.Text = dtmusolo.Rows[0][1].ToString();
            label18.Text = dtmusolo.Rows[0][2].ToString();
            label19.Text = dtmusolo.Rows[0][3].ToString();
            label20.Text = dtmusolo.Rows[0][4].ToString();
            label22.Text = dtmusolo.Rows[0][5].ToString();



        }

        private void teamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dtteam.Clear();
            dttuanrumah.Clear();
            dttamu.Clear();    
            dtselected.Clear();

            query = "select team_name,team_id from team;";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dtteam);
            comboBox3.DataSource = dtteam;
            comboBox3.ValueMember = "team_id";
            comboBox3.DisplayMember = "team_name";
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dtselected.Clear();
            dttuanrumah.Clear();
            dttamu.Clear();
            query = $"select d.match_id from dmatch d, `match` m , `match` n WHERE d.match_id = m.match_id AND (m.team_home = d.team_id or m.team_away = d.team_id) AND d.team_id = '{comboBox3.SelectedValue.ToString()}';";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dtselected);
            comboBox4.DataSource = dtselected;
            comboBox4.ValueMember = "match_id";
            comboBox4.DisplayMember = "match_id";
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dttuanrumah.Clear();
            dttamu.Clear();
            query = $"select p.player_name,t.team_name,if(p.playing_pos = 'F','Forward',if(p.playing_pos = 'M','Midfielder',if (p.playing_pos = 'D','Defender','Goalkeeper')))as'position' from player p, team t , `match` m where t.team_id = m.team_home and t.team_id = p.team_id and m.match_id = '{comboBox4.SelectedValue.ToString()}';";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dttuanrumah);
            dataGridView1.DataSource = dttuanrumah;

          
            query = $"select p.player_name,t.team_name,if(p.playing_pos = 'F','Forward',if(p.playing_pos = 'M','Midfielder',if (p.playing_pos = 'D','Defender','Goalkeeper')))as'position' from player p, team t , `match` m where t.team_id = m.team_away and t.team_id = p.team_id and m.match_id = '{comboBox4.SelectedValue.ToString()}';";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dttamu);
            dataGridView2.DataSource = dttamu;

            dtcadangan.Clear();
            query = $"select d.`minute`,p.player_name,t.team_name,if(d.`type`='CY','Yellow Card',if(d.`type`='CR','Red Card',if(d.`type`='GO','Goal',if(d.`type`='GW','Own Goal',if(d.`type`='GP','Goal Penalty','Penalty Missed')))))as'type' from player p, team t , `match` m, dmatch d where t.team_id = d.team_id and d.player_id = p.player_id and d.match_id = '{comboBox4.SelectedValue.ToString()}';";
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            adapter.Fill(dtcadangan);
            dataGridView3.DataSource = dtcadangan;
            
        }
    }
}
