using Microsoft.VisualBasic.ApplicationServices;
using System.Media;
using WinFormsApp2.Properties;
namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        //宣告判斷向左、向右的布林值
        bool goleft, goright;
        //宣告初始的成績
        int score = 0;
        //宣告沒有接到元寶的數量
        int missed = 0;
        //宣告遊戲準備秒數的時間
        int duration = 5;
        //元寶、黃金、鞭炮隨機生成之掉落處實例
        Random randX = new Random();
        Random randY = new Random();
        //宣告元寶掉落時會變成大便的pictureBox控制項
        PictureBox drop = new PictureBox();
        
        public Form1()
        {

            InitializeComponent();
            RestartGame();
            restartgold();
            restartbomb();

        }

        //KeyDown事件，按下鍵盤左、右鍵將true傳到goleft、goright
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
        }
        //KeyUp事件，放開鍵盤左、右鍵將false傳到goleft、goright
        private void KeyIsUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
        }
        
        private void starttimer_Tick(object sender, EventArgs e)
        {
            preparetime();
        }
        //當倒數計時大於0，角色不能動；當倒數計時為0時，呼叫process方法
        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            if(duration > 0)
            {
                goleft = false;
                goright = false;
            }
            if(duration == 0)
            {
                process();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelstyle();
            this.MaximumSize = new System.Drawing.Size(570, 760);
            this.MinimumSize = new System.Drawing.Size(570, 760);
        }

        private void RestartGame()
        {
            /*透過 x 來檢查出現在 windows form 的每個控制項是否符合pictureBox以及tag為money。
             如果是，則randY在-300~-80生成亂數，傳給x所在的高度；同時，在寬度5~表單-x寬度的範圍生成亂數，傳給x所在的水平位置*/

            foreach (Control x in this.Controls)
                if (x is PictureBox && (string)x.Tag == "money")
                {
                    x.Top = randY.Next(80, 300) * -1;
                    x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                }
            //設定player的初始水平位置在表單的置中處，垂直位置在表單y軸的580
            player.Location = new Point(this.ClientSize.Width / 2, 580);
            //設定player的圖像
            player.Image = Properties.Resources.containerleft;

            //初始化分數、未接數量、速度
            score = 0;
            missed = 0;
            //防止重新開始遊戲時，角色亂移動
            goleft = false;
            goright = false;
            starttimer.Start();
            //如果GameTimer開始，清除drop
            if (GameTimer.Enabled == true)
            {
                drop.Visible = false;
            }
            //重新開始GameTimer計數器
            GameTimer.Start();
            
        }
        //讓黃金能重新降下的方法
        private void restartgold()
        {
            foreach (Control x in this.Controls)
                if (x is PictureBox && (string)x.Tag == "gold")
                {
                    x.Top = randY.Next(80, 300) * -2;
                    x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                }
        }
       
        //讓鞭炮重新降下的方法
        private void restartbomb()
        {
            foreach (Control x in this.Controls)
                if (x is PictureBox && (string)x.Tag == "bomb")
                {
                    x.Top = randY.Next(80, 300) * -1;
                    x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                }
        }
      

        private void process()
        {
            //動態調整遊玩時的分數、未接到元寶以及黃金的數量
            textscore.Text = "Score:" + score;
            textmiss.Text = "Missed" + missed;
            //若goleft為true且角色沒有在視窗的邊界，角色左移動12；同時，設定角色的圖檔
            if (goleft == true && player.Left > 0)
            {
                player.Left -= 12;
                player.Image = Properties.Resources.containerleft;
            }
            //若goright為true且角色左邊的空間+角色的寬度<視窗的寬度，角色右移動12；同時，設定角色的圖檔
            if (goright == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += 12;
                player.Image = Properties.Resources.containerright;
            }
            /*透過 x 來檢查出現在 windows form 的每個控制項是否符合pictureBox以及tag為money。
             如果是，則x加上speed；如果x物體的高度+x物體所在的垂直位置>表單底部，則將原本的元寶圖片替換成大便的圖片，財神爺的圖片替換成窮神
            randY在-300~-80生成亂數，傳給x所在的高度；同時，在寬度5~表單-x寬度的範圍生成亂數，傳給x所在的水平位置*/
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "money")
                {
                    x.Top += 8;
                    if (x.Top + x.Height > this.ClientSize.Height)
                    {
                        //設定圖片可以看到
                        drop.Visible = true;
                        //設定圖片可以延展
                        drop.SizeMode = PictureBoxSizeMode.StretchImage;
                        drop.Image = Properties.Resources.drop;
                        //設定圖片的水平未置於原先元寶隨機生成處，垂直位置為652
                        drop.Location = new Point(x.Left , 652);
                        drop.Height = 80;
                        drop.Width = 60;
                        drop.BackColor= Color.Transparent;
                        //在表單新增drop
                        this.Controls.Add(drop);
                        x.Top = randY.Next(80, 300) * -1;
                        x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                        //未接數量加1
                        missed += 1;
                        //將財神爺轉成窮神
                        player.Image = Properties.Resources.bowl;
                    }
                    //如果元寶碰到財神爺，分數加1
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        x.Top = randY.Next(80, 300) * -1;
                        x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                        score += 1;
                    }
                }
            }

            if (score > 2)
            {
            /*透過 x 來檢查出現在 windows form 的每個控制項是否符合pictureBox以及tag為bomb。
             如果是，則x加上speed；如果x物體的高度+x物體所在的垂直位置>表單底部，則將原本的元寶圖片替換成大便的圖片，財神爺的圖片替換成窮神
            randY在-300~-80生成亂數，傳給x所在的高度；同時，在寬度5~表單-x寬度的範圍生成亂數，傳給x所在的水平位置*/
                foreach (Control x in this.Controls)
                {

                    if (x is PictureBox && (string)x.Tag == "bomb")
                    {
                        //代表物體下降的速度
                        x.Top += 10;
                        //當物體在表單上方的高度+物體本身的高度>表單的高度，隨機在表單中高度-300~-80的地方開始下降；從5開始到表單的邊界開始下降
                        if (x.Top + x.Height > this.ClientSize.Height)
                        {
                            x.Top = randY.Next(80, 300) * -1;
                            x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                            
                        }
                        //如果角色碰到鞭炮，GameTimer計數器停止
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            x.Top = randY.Next(80, 300) * -1;
                            x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                            GameTimer.Stop();
                            //跳出MessageBox，按下確認後，重置黃金、炸彈、元寶
                            MessageBox.Show("Game Over!");
                            restartgold();
                            restartbomb();
                            RestartGame();
                        }
                    }
                }
            }
            if(score >1)
            {
                foreach (Control x in this.Controls)
                {

                    if (x is PictureBox && (string)x.Tag == "gold")
                    {
                        //代表物體會從表單最上方下降
                        x.Top += 14;
                        //當物體在表單上方的高度+物體本身的高度>表單的高度，隨機在表單中高度80~300的地方開始下降；從5開始到扁擔的邊界開始下降
                        if (x.Top + x.Height > this.ClientSize.Height)
                        {
                            x.Top = randY.Next(80, 300) * -2;
                            x.Left = randX.Next(5, this.ClientSize.Width - x.Width);

                        }
                        //如果黃金碰到財神爺，分數加2
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            x.Top = randY.Next(80, 300) * -1;
                            x.Left = randX.Next(5, this.ClientSize.Width - x.Width);
                            score += 2;
                        }
                    }
                    //調整元寶掉落的速度
                    if (x is PictureBox && (string)x.Tag == "money")
                    {
                        x.Top += 2;
                    }
                }
            }
            //如果分數超過，顯示MessageBox:你贏了，停止GameTimer計時器並退出視窗
            if(score > 3)
            {
                GameTimer.Stop();
                MessageBox.Show("Win");
                Application.Exit();
            }
            //如果missed的數量超過，計時器GameTimer停止，並顯示MessageBox，drop隱藏，重新開始遊戲、元寶掉落、炸彈掉落
            if (missed > 100)
            {
                GameTimer.Stop();
                MessageBox.Show("Game Over!");
                drop.Visible = false;
                RestartGame();
                restartgold();
                restartbomb();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //設定遊戲開始前的5秒準備時間
        private void preparetime()
        {
            //如果duration變數為0，停止starttimer計時器，並讓label1隱藏
            if (duration == 0)
            {
                starttimer.Stop();
                label1.Visible = false;
            }
            //如果duration變數>0，duration遞減，並顯示在label1
            else if (duration > 0)
            {
                duration--;
                label1.Text = duration.ToString();
            }
        }

        //label的樣式方法
        private void labelstyle()
        {
            label1.Font = new Font("Times New Roman", 28, FontStyle.Bold);
        }
    }
}