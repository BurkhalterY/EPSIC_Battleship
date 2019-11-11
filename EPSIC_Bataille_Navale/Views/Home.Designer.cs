namespace EPSIC_Bataille_Navale.Views
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_solo = new System.Windows.Forms.Button();
            this.lbl_title = new System.Windows.Forms.Label();
            this.btn_online = new System.Windows.Forms.Button();
            this.btn_credits = new System.Windows.Forms.Button();
            this.txt_pseudo = new System.Windows.Forms.TextBox();
            this.lbl_pseudo = new System.Windows.Forms.Label();
            this.btn_demo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_solo
            // 
            this.btn_solo.Location = new System.Drawing.Point(253, 253);
            this.btn_solo.Name = "btn_solo";
            this.btn_solo.Size = new System.Drawing.Size(75, 23);
            this.btn_solo.TabIndex = 0;
            this.btn_solo.Text = "Solo";
            this.btn_solo.UseVisualStyleBackColor = true;
            this.btn_solo.Click += new System.EventHandler(this.Btn_solo_Click);
            // 
            // lbl_title
            // 
            this.lbl_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(3, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(744, 250);
            this.lbl_title.TabIndex = 1;
            this.lbl_title.Text = "Battleship!";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btn_online
            // 
            this.btn_online.Location = new System.Drawing.Point(334, 253);
            this.btn_online.Name = "btn_online";
            this.btn_online.Size = new System.Drawing.Size(75, 23);
            this.btn_online.TabIndex = 2;
            this.btn_online.Text = "Online";
            this.btn_online.UseVisualStyleBackColor = true;
            this.btn_online.Click += new System.EventHandler(this.Btn_online_Click);
            // 
            // btn_credits
            // 
            this.btn_credits.Location = new System.Drawing.Point(415, 253);
            this.btn_credits.Name = "btn_credits";
            this.btn_credits.Size = new System.Drawing.Size(75, 23);
            this.btn_credits.TabIndex = 4;
            this.btn_credits.Text = "Crédits";
            this.btn_credits.UseVisualStyleBackColor = true;
            this.btn_credits.Click += new System.EventHandler(this.Btn_credits_Click);
            // 
            // txt_pseudo
            // 
            this.txt_pseudo.Location = new System.Drawing.Point(334, 282);
            this.txt_pseudo.Name = "txt_pseudo";
            this.txt_pseudo.Size = new System.Drawing.Size(156, 20);
            this.txt_pseudo.TabIndex = 5;
            // 
            // lbl_pseudo
            // 
            this.lbl_pseudo.AutoSize = true;
            this.lbl_pseudo.ForeColor = System.Drawing.Color.White;
            this.lbl_pseudo.Location = new System.Drawing.Point(250, 285);
            this.lbl_pseudo.Name = "lbl_pseudo";
            this.lbl_pseudo.Size = new System.Drawing.Size(61, 13);
            this.lbl_pseudo.TabIndex = 6;
            this.lbl_pseudo.Text = "Votre nom :";
            // 
            // btn_demo
            // 
            this.btn_demo.Location = new System.Drawing.Point(42, 443);
            this.btn_demo.Name = "btn_demo";
            this.btn_demo.Size = new System.Drawing.Size(156, 23);
            this.btn_demo.TabIndex = 7;
            this.btn_demo.Text = "Démo (IA vs. IA)";
            this.btn_demo.UseVisualStyleBackColor = true;
            this.btn_demo.Click += new System.EventHandler(this.Btn_demo_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_demo);
            this.Controls.Add(this.lbl_pseudo);
            this.Controls.Add(this.txt_pseudo);
            this.Controls.Add(this.btn_credits);
            this.Controls.Add(this.btn_online);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.btn_solo);
            this.Name = "Home";
            this.Size = new System.Drawing.Size(750, 500);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_solo;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Button btn_online;
        private System.Windows.Forms.Button btn_credits;
        private System.Windows.Forms.TextBox txt_pseudo;
        private System.Windows.Forms.Label lbl_pseudo;
        private System.Windows.Forms.Button btn_demo;
    }
}