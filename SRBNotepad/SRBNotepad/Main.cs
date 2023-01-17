using System.IO;
using System.Windows.Forms;

namespace Notepad_v2
{
    public partial class Main : Form
    {
        public string filename;
        public bool isFileChanged;

        public Main()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            filename = "";
            isFileChanged = false;
            UpdateTitleWithText();
        }

        public void SaveUnsavedFile()
        {
            if (isFileChanged)
            {
                string saveMessage = "Сохранить изменения в файле?";
                string saveCaption = "Сохранение файла";
                DialogResult result = MessageBox.Show(saveMessage, saveCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { SaveFile(filename); }
            }
        }

        public void CreateNewDocument(object sender, EventArgs e)
        {
            SaveUnsavedFile();
            richTextBox1.Text = "";
            filename = "";
            isFileChanged = false;
            UpdateTitleWithText();
        }

        public void OpenFile(object sender,EventArgs e)
        {
            openFileDialog1.Filter = "Текстовые документы (*.txt)|*.txt|Все форматы (*.*)|*.*";
            SaveUnsavedFile();
            openFileDialog1.FileName = "";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    richTextBox1.Text = sr.ReadToEnd();
                    sr.Close();
                    filename = openFileDialog1.FileName;
                    isFileChanged = false;
                }
                catch
                {
                    string errorMessage = "Невозможно открыть файл!";
                    string errorCaption = "Ошибка";
                    DialogResult result = MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                UpdateTitleWithText();
            }
        }

        public void SaveFile(string _filename)
        {
            saveFileDialog1.Filter = "Текстовые документы (*.txt)|*.txt|Все форматы (*.*)|*.*";
            if (_filename == "")
            {
                if(saveFileDialog1.ShowDialog() == DialogResult.OK) { _filename = saveFileDialog1.FileName; }
            }
            try
            {
                StreamWriter sw = new StreamWriter(_filename + ".txt");
                sw.Write(richTextBox1.Text);
                sw.Close();
                filename = _filename;
                isFileChanged = false;
            }
            catch
            {
                string errorMessage = "Невозможно сохранить файл!";
                string errorCaption = "Ошибка";
                DialogResult result = MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            UpdateTitleWithText();
        }
        
        public void Save(object sender, EventArgs e) { SaveFile(filename); }
        public void SaveAs(object sender, EventArgs e) { SaveFile(""); }
        private void OnFormClosing(object sender, FormClosingEventArgs e) { SaveUnsavedFile(); }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!isFileChanged)
            {
                this.Text = this.Text.Replace("*", " ");
                isFileChanged = true;
                this.Text = "*" + this.Text;
            }
        }

        public void UpdateTitleWithText()
        {
            if (filename != "")
                this.Text = filename + " - SRBNotepad";
            else this.Text = "Документ без названия" + " - SRBNotepad";
        }

        private void OnCopyClick(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                Clipboard.SetText(richTextBox1.SelectedText);
            }
        }

        private void OnPasteClick(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.SelectionStart) +
               Clipboard.GetText() + richTextBox1.Text.Substring(richTextBox1.SelectionStart,
               richTextBox1.Text.Length - richTextBox1.SelectionStart);
        }

        private void OnCutClick(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                Clipboard.SetText(richTextBox1.SelectedText);
                richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart, richTextBox1.SelectionLength);
            }
        }

        private void FontSettings(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.Font = fontDialog1.Font;
        }

        private void AboutProgram(object sender, EventArgs e)
        {
            AboutProgram aboutProgram = new();
            aboutProgram.ShowDialog();
        }
    }
}