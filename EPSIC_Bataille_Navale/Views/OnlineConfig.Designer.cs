namespace EPSIC_Bataille_Navale.Views
{
    partial class OnlineConfig
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_host = new System.Windows.Forms.Button();
            this.btn_join = new System.Windows.Forms.Button();
            this.txt_serv_address = new System.Windows.Forms.TextBox();
            this.lbl_serv_address = new System.Windows.Forms.Label();
            this.btn_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_host
            // 
            this.btn_host.Location = new System.Drawing.Point(380, 226);
            this.btn_host.Name = "btn_host";
            this.btn_host.Size = new System.Drawing.Size(164, 49);
            this.btn_host.TabIndex = 0;
            this.btn_host.Text = "Host game";
            this.btn_host.UseVisualStyleBackColor = true;
            this.btn_host.Click += new System.EventHandler(this.Btn_host_Click);
            // 
            // btn_join
            // 
            this.btn_join.Location = new System.Drawing.Point(210, 252);
            this.btn_join.Name = "btn_join";
            this.btn_join.Size = new System.Drawing.Size(164, 23);
            this.btn_join.TabIndex = 1;
            this.btn_join.Text = "Join game";
            this.btn_join.UseVisualStyleBackColor = true;
            this.btn_join.Click += new System.EventHandler(this.Btn_join_Click);
            // 
            // txt_serv_address
            // 
            this.txt_serv_address.Location = new System.Drawing.Point(274, 226);
            this.txt_serv_address.Name = "txt_serv_address";
            this.txt_serv_address.Size = new System.Drawing.Size(100, 20);
            this.txt_serv_address.TabIndex = 2;
            // 
            // lbl_serv_address
            // 
            this.lbl_serv_address.AutoSize = true;
            this.lbl_serv_address.Location = new System.Drawing.Point(207, 229);
            this.lbl_serv_address.Name = "lbl_serv_address";
            this.lbl_serv_address.Size = new System.Drawing.Size(61, 13);
            this.lbl_serv_address.TabIndex = 3;
            this.lbl_serv_address.Text = "IP serveur :";
            // 
            // btn_back
            // 
            this.btn_back.Location = new System.Drawing.Point(3, 474);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(75, 23);
            this.btn_back.TabIndex = 4;
            this.btn_back.Text = "Retour";
            this.btn_back.UseVisualStyleBackColor = true;
            this.btn_back.Click += new System.EventHandler(this.Btn_back_Click);
            // 
            // OnlineConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.lbl_serv_address);
            this.Controls.Add(this.txt_serv_address);
            this.Controls.Add(this.btn_join);
            this.Controls.Add(this.btn_host);
            this.Name = "OnlineConfig";
            this.Size = new System.Drawing.Size(750, 500);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_host;
        private System.Windows.Forms.Button btn_join;
        private System.Windows.Forms.TextBox txt_serv_address;
        private System.Windows.Forms.Label lbl_serv_address;
        private System.Windows.Forms.Button btn_back;
    }
}
