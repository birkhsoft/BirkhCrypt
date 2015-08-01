Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

'
' .----------------.  .----------------.  .----------------.  .----------------.  .----------------.  .----------------.  .----------------.  .----------------. 
'| .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |
'| |   ______     | || |     _____    | || |  _______     | || |  ___  ____   | || |  ____  ____  | || |     ____     | || |  _________   | || |  _________   | |
'| |  |_   _ \    | || |    |_   _|   | || | |_   __ \    | || | |_  ||_  _|  | || | |_   ||   _| | || |   .'    '.   | || | |_   ___  |  | || | |_   ___  |  | |
'| |    | |_) |   | || |      | |     | || |   | |__) |   | || |   | |_/ /    | || |   | |__| |   | || |  |  .--.  |  | || |   | |_  \_|  | || |   | |_  \_|  | |
'| |    |  __'.   | || |      | |     | || |   |  __ /    | || |   |  __'.    | || |   |  __  |   | || |  | |    | |  | || |   |  _|      | || |   |  _|      | |
'| |   _| |__) |  | || |     _| |_    | || |  _| |  \ \_  | || |  _| |  \ \_  | || |  _| |  | |_  | || |  |  `--'  |  | || |  _| |_       | || |  _| |_       | |
'| |  |_______/   | || |    |_____|   | || | |____| |___| | || | |____||____| | || | |____||____| | || |   '.____.'   | || | |_____|      | || | |_____|      | |
'| |              | || |              | || |              | || |              | || |              | || |              | || |              | || |              | |
'| '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |
''----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'  '----------------'



Public Class Form1
    Private Const sSecretKey As String = "Password"

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btn_crypt.Click
        EncryptFile(TextBox1.Text,
                        TextBox1.Text & ".crypt6",
                        sSecretKey)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btn_decrypt.Click
        DecryptFile(TextBox1.Text,
                    TextBox1.Text & ".decrypt6",
                    sSecretKey)
    End Sub

    Sub EncryptFile(ByVal sInputFilename As String, _
                  ByVal sOutputFilename As String, _
                  ByVal sKey As String)

        Dim fsInput As New FileStream(sInputFilename, _
                                    FileMode.Open, FileAccess.Read)
        Dim fsEncrypted As New FileStream(sOutputFilename, _
                                    FileMode.Create, FileAccess.Write)

        Dim DES As New DESCryptoServiceProvider()

        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey)

        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey)

        Dim desencrypt As ICryptoTransform = DES.CreateEncryptor()
        Dim cryptostream As New CryptoStream(fsEncrypted,
                                            desencrypt,
                                            CryptoStreamMode.Write)

        Dim bytearrayinput(fsInput.Length - 1) As Byte
        fsInput.Read(bytearrayinput, 0, bytearrayinput.Length)
        cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length)
        cryptostream.Close()
    End Sub

    Sub DecryptFile(ByVal sInputFilename As String, _
        ByVal sOutputFilename As String, _
        ByVal sKey As String)

        Dim DES As New DESCryptoServiceProvider()
        DES.Key() = ASCIIEncoding.ASCII.GetBytes(sKey)
        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey)

        Dim fsread As New FileStream(sInputFilename, FileMode.Open, FileAccess.Read)
        Dim desdecrypt As ICryptoTransform = DES.CreateDecryptor()
        Dim cryptostreamDecr As New CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read)
        Dim fsDecrypted As New StreamWriter(sOutputFilename)
        fsDecrypted.Write(New StreamReader(cryptostreamDecr).ReadToEnd)
        fsDecrypted.Flush()
        fsDecrypted.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "c:\"
        openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                myStream = openFileDialog1.OpenFile()
                If (myStream IsNot Nothing) Then
                End If
                TextBox1.Text = openFileDialog1.FileName
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally

                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If
    End Sub


End Class
