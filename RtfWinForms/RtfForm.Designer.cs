namespace Itenso.Solutions.Community.RtfConverter.RtfWinForms
{
	partial class RtfForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.textBox = new System.Windows.Forms.TextBox();
			this.toTextButton = new System.Windows.Forms.Button();
			this.toXmlButton = new System.Windows.Forms.Button();
			this.toHtmlButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 3;
			this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 33.33333F ) );
			this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 33.33333F ) );
			this.tableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 33.33333F ) );
			this.tableLayoutPanel.Controls.Add( this.richTextBox, 0, 0 );
			this.tableLayoutPanel.Controls.Add( this.textBox, 0, 2 );
			this.tableLayoutPanel.Controls.Add( this.toTextButton, 0, 1 );
			this.tableLayoutPanel.Controls.Add( this.toXmlButton, 1, 1 );
			this.tableLayoutPanel.Controls.Add( this.toHtmlButton, 2, 1 );
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point( 0, 0 );
			this.tableLayoutPanel.MinimumSize = new System.Drawing.Size( 600, 400 );
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 3;
			this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
			this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle() );
			this.tableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
			this.tableLayoutPanel.Size = new System.Drawing.Size( 689, 448 );
			this.tableLayoutPanel.TabIndex = 0;
			// 
			// richTextBox
			// 
			this.tableLayoutPanel.SetColumnSpan( this.richTextBox, 3 );
			this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox.Location = new System.Drawing.Point( 3, 3 );
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.Size = new System.Drawing.Size( 683, 203 );
			this.richTextBox.TabIndex = 0;
			this.richTextBox.Text = "";
			// 
			// textBox
			// 
			this.tableLayoutPanel.SetColumnSpan( this.textBox, 3 );
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point( 3, 241 );
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size( 683, 204 );
			this.textBox.TabIndex = 1;
			// 
			// toTextButton
			// 
			this.toTextButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toTextButton.Location = new System.Drawing.Point( 3, 212 );
			this.toTextButton.Name = "toTextButton";
			this.toTextButton.Size = new System.Drawing.Size( 223, 23 );
			this.toTextButton.TabIndex = 2;
			this.toTextButton.Text = "To Text";
			this.toTextButton.UseVisualStyleBackColor = true;
			this.toTextButton.Click += new System.EventHandler( this.ToTextButtonClick );
			// 
			// toXmlButton
			// 
			this.toXmlButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toXmlButton.Location = new System.Drawing.Point( 232, 212 );
			this.toXmlButton.Name = "toXmlButton";
			this.toXmlButton.Size = new System.Drawing.Size( 223, 23 );
			this.toXmlButton.TabIndex = 3;
			this.toXmlButton.Text = "To Xml";
			this.toXmlButton.UseVisualStyleBackColor = true;
			this.toXmlButton.Click += new System.EventHandler( this.ToXmlButtonClick );
			// 
			// toHtmlButton
			// 
			this.toHtmlButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toHtmlButton.Location = new System.Drawing.Point( 461, 212 );
			this.toHtmlButton.Name = "toHtmlButton";
			this.toHtmlButton.Size = new System.Drawing.Size( 225, 23 );
			this.toHtmlButton.TabIndex = 4;
			this.toHtmlButton.Text = "To Html";
			this.toHtmlButton.UseVisualStyleBackColor = true;
			this.toHtmlButton.Click += new System.EventHandler( this.ToHtmlButtonClick );
			// 
			// RtfForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 689, 448 );
			this.Controls.Add( this.tableLayoutPanel );
			this.Name = "RtfForm";
			this.Text = "RTF Converter for Windows Forms";
			this.tableLayoutPanel.ResumeLayout( false );
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.RichTextBox richTextBox;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Button toTextButton;
		private System.Windows.Forms.Button toXmlButton;
		private System.Windows.Forms.Button toHtmlButton;


	}
}

