namespace WinFormDemo
{
    partial class EncoderDecoder
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
            this.cbEncodeType = new System.Windows.Forms.ComboBox();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.txtEncoder = new System.Windows.Forms.TextBox();
            this.txtDecoder = new System.Windows.Forms.TextBox();
            this.btnEncDec = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbEncodeType
            // 
            this.cbEncodeType.FormattingEnabled = true;
            this.cbEncodeType.Location = new System.Drawing.Point(28, 12);
            this.cbEncodeType.Name = "cbEncodeType";
            this.cbEncodeType.Size = new System.Drawing.Size(256, 20);
            this.cbEncodeType.TabIndex = 0;
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(28, 64);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(256, 88);
            this.txtContent.TabIndex = 1;
            // 
            // txtEncoder
            // 
            this.txtEncoder.Location = new System.Drawing.Point(28, 178);
            this.txtEncoder.Multiline = true;
            this.txtEncoder.Name = "txtEncoder";
            this.txtEncoder.Size = new System.Drawing.Size(256, 88);
            this.txtEncoder.TabIndex = 2;
            // 
            // txtDecoder
            // 
            this.txtDecoder.Location = new System.Drawing.Point(28, 295);
            this.txtDecoder.Multiline = true;
            this.txtDecoder.Name = "txtDecoder";
            this.txtDecoder.Size = new System.Drawing.Size(256, 88);
            this.txtDecoder.TabIndex = 3;
            // 
            // btnEncDec
            // 
            this.btnEncDec.Location = new System.Drawing.Point(100, 389);
            this.btnEncDec.Name = "btnEncDec";
            this.btnEncDec.Size = new System.Drawing.Size(75, 23);
            this.btnEncDec.TabIndex = 5;
            this.btnEncDec.Text = "编码/解码";
            this.btnEncDec.UseVisualStyleBackColor = true;
            this.btnEncDec.Click += new System.EventHandler(this.btnEncDec_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 278);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "解码后：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "内容：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "编码后：";
            // 
            // EncoderDecoder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 420);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEncDec);
            this.Controls.Add(this.txtDecoder);
            this.Controls.Add(this.txtEncoder);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.cbEncodeType);
            this.Name = "EncoderDecoder";
            this.Text = "EncoderDecoder";
            this.Load += new System.EventHandler(this.EncoderDecoder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbEncodeType;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.TextBox txtEncoder;
        private System.Windows.Forms.TextBox txtDecoder;
        private System.Windows.Forms.Button btnEncDec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}