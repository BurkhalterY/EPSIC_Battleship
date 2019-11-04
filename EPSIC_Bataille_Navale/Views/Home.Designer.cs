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
            this.SuspendLayout();
            // 
            // btn_solo
            // 
            this.btn_solo.Location = new System.Drawing.Point(189, 268);
            this.btn_solo.Name = "btn_solo";
            this.btn_solo.Size = new System.Drawing.Size(75, 23);
            this.btn_solo.TabIndex = 0;
            this.btn_solo.Text = "Solo";
            this.btn_solo.UseVisualStyleBackColor = true;
            this.btn_solo.Click += new System.EventHandler(this.Btn_solo_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_solo);
            this.Name = "Home";
            this.Size = new System.Drawing.Size(500, 500);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_solo;
    }
}