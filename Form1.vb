Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Drawing
Imports System.Net
Imports System.Net.Http
Imports Microsoft.Web.Administration
Imports System.Runtime.InteropServices.JavaScript.JSType
Imports System.Security.AccessControl
Public Class Form1
    Private WithEvents client As New WebClient()
    Dim DataAIFolder As String = "DataAI"
    Dim downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")   'txtyourfoldertouploadfiles.Text
    Dim errors As String = String.Empty
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.SizeGripStyle = SizeGripStyle.Auto
        Me.MinimumSize = New Size(600, 400) ' optional

        If txtyoururlDataAI.Text.Trim <> "" AndAlso txtyoururlDataAI.Text.Trim <> "[your web url for DataAI]" Then
            DataAIFolder = txtyoururlDataAI.Text.Trim
            If DataAIFolder.Trim <> "" AndAlso DataAIFolder <> "[your web url for DataAI]" Then
                If DataAIFolder.LastIndexOf("/") = DataAIFolder.Length - 1 Then
                    DataAIFolder = DataAIFolder.Substring(0, DataAIFolder.Length - 1)
                End If
                DataAIFolder = DataAIFolder.Substring(DataAIFolder.LastIndexOf("/")).Replace("/", "").Replace(" ", "")
            End If
        End If
        txtyourfoldertouploadfiles.Text = downloadsPath
        txtyourfoldertouploadfiles.Enabled = False
        'restore previous setting
        Dim ret As String = ShowPreviousSetting(DataAIFolder)

    End Sub
    Private Function ShowPreviousSetting(ByVal foldername As String) As String
        If Not Directory.Exists("C:\inetpub\wwwroot") Then
            MsgBox("Directory C:\inetpub\wwwroot\ does not exist. Install Internet Information Services (IIS) Manager on your Windows computer first")
            txtyoururlDataAI.BackColor = Color.Pink
            Exit Function
        ElseIf Not Directory.Exists("C:\inetpub\wwwroot\" & foldername) OrElse txtyoururlDataAI.Text.Trim = "" Then
            'MsgBox("Directory " & foldername & " does not exist. Download and Install It first")
            txtyoururlDataAI.BackColor = Color.Pink
            'txtyourfoldertouploadfiles.BackColor = Color.Pink
            Return ""
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If

        'restore previous setting
        Dim webconfigpath As String = "C:\inetpub\wwwroot\" & DataAIFolder & "\web.config"
        Dim weborgconfigpath As String = "C:\inetpub\wwwroot\" & DataAIFolder & "\webOriginal.config"
        'Dim wcfg As String = String.Empty
        'Dim firsttimesetting As Boolean = False
        If File.Exists(weborgconfigpath) Then  'original web config exists, web.config has previous setting written in the bottom of the file
            Dim sr() As String = File.ReadAllLines(webconfigpath)
            For i = 0 To sr.Length - 1
                If sr(i).Contains("//[operational database server]") Then
                    txtOperationalDatabaseServer.Text = sr(i).Replace("//[operational database server]", "").Trim
                ElseIf sr(i).Contains("//[operational database name]") Then
                    txtOperationalDatabaseName.Text = sr(i).Replace("//[operational database name]", "").Trim
                ElseIf sr(i).Contains("//[operational database user ID]") Then
                    txtoperationaldatabaseuserID.Text = sr(i).Replace("//[operational database user ID]", "").Trim
                ElseIf sr(i).Contains("//[operational database password]") Then
                    txtoperationaldatabasepassword.Text = sr(i).Replace("//[operational database password]", "").Trim
                ElseIf sr(i).Contains("//[operational database provider name]") Then
                    txtoperationaldatabaseprovider.Text = sr(i).Replace("//[operational database provider name]", "").Trim
                ElseIf sr(i).Contains("//[operational database port if needed]") Then
                    txtoperationaldatabaseport.Text = sr(i).Replace("//[operational database port if needed]", "").Trim
                ElseIf sr(i).Contains("//[system database password if needed]") Then
                    txtsystemdatabasepassword.Text = sr(i).Replace("//[system database password if needed]", "").Trim

                ElseIf sr(i).Contains("//[your data server]") Then
                    txtyourdataserver.Text = sr(i).Replace("//[your data server]", "").Trim
                ElseIf sr(i).Contains("//[your database name]") Then
                    txtyourDatabaseName.Text = sr(i).Replace("//[your database name]", "").Trim
                ElseIf sr(i).Contains("//[your database user ID]") Then
                    txtyourdatabaseuserID.Text = sr(i).Replace("//[your database user ID]", "").Trim
                ElseIf sr(i).Contains("//[your database password]") Then
                    txtyourdatabasepassword.Text = sr(i).Replace("//[your database password]", "").Trim
                ElseIf sr(i).Contains("//[your database provider name]") Then
                    txtyourdatabaseprovider.Text = sr(i).Replace("//[your database provider name]", "").Trim
                ElseIf sr(i).Contains("//[your database port if needed]") Then
                    txtyourdatabaseport.Text = sr(i).Replace("//[your database port if needed]", "").Trim

                ElseIf sr(i).Contains("//[your smtp email]") Then
                    txtyoursmtpemail.Text = sr(i).Replace("//[your smtp email]", "").Trim
                ElseIf sr(i).Contains("//[password for your smtp email]") Then
                    txtyoursmtpemailpassword.Text = sr(i).Replace("//[password for your smtp email]", "").Trim

                ElseIf sr(i).Contains("//[your web url for DataAI]") Then
                    txtyoururlDataAI.Text = sr(i).Replace("//[your web url for DataAI]", "").Trim
                ElseIf sr(i).Contains("//[your web site title]") Then
                    txtyourwebsitetitle.Text = sr(i).Replace("//[your web site title]", "").Trim
                ElseIf sr(i).Contains("//[your support email]") Then
                    txtyoursupportemail.Text = sr(i).Replace("//[your support email]", "").Trim
                    'ElseIf sr(i).Contains("//[your folder to upload files]") Then
                    '    txtyourfoldertouploadfiles.Text = sr(i).Replace("//[your folder to upload files]", "").Trim
                ElseIf sr(i).Contains("//[your google map key]") Then
                    txtyourgooglemapkey.Text = sr(i).Replace("//[your google map key]", "").Trim

                ElseIf sr(i).Contains("//[your OpenAI key]") Then
                    txtyourOpenAIkey.Text = sr(i).Replace("//[your OpenAI key]", "").Trim
                ElseIf sr(i).Contains("//[OpenAI model]") Then
                    txtyourOpenAImodel.Text = sr(i).Replace("//[OpenAI model]", "").Trim
                ElseIf sr(i).Contains("//[OpenAI maximum tokens limit]") Then
                    txtyourmaximumtokens.Text = sr(i).Replace("//[OpenAI maximum tokens limit]", "").Trim
                ElseIf sr(i).Contains("//[your OpenAI organization code]") Then
                    txtyourOpenAIorganizationcode.Text = sr(i).Replace("//[your OpenAI organization code]", "").Trim
                End If
            Next
        End If
        Return ""
    End Function
    Private Sub btnInstall_Click(sender As Object, e As EventArgs)
        Dim ret = String.Empty
        If txtyoururlDataAI.Text.Trim = "" OrElse cleanTextShort(txtyoururlDataAI.Text) <> txtyoururlDataAI.Text.Trim Then
            txtyoururlDataAI.BackColor = Color.Pink
            MsgBox("Wrong values in textboxes")
            Exit Sub
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If
        'If txtyourfoldertouploadfiles.Text.Trim = "" OrElse cleanTextShort(txtyourfoldertouploadfiles.Text) <> txtyourfoldertouploadfiles.Text Then
        '    txtyourfoldertouploadfiles.BackColor = Color.Pink
        '    MsgBox("Wrong values in textboxes")
        '    Exit Sub
        'Else
        '    txtyourfoldertouploadfiles.BackColor = Color.White
        'End If
        If txtyoururlDataAI.Text.Trim <> "" AndAlso txtyoururlDataAI.Text.Trim <> "[your web url for DataAI]" Then
            DataAIFolder = txtyoururlDataAI.Text.Trim
            If DataAIFolder.Trim <> "" AndAlso DataAIFolder <> "[your web url for DataAI]" Then
                If DataAIFolder.LastIndexOf("/") = DataAIFolder.Length - 1 Then
                    DataAIFolder = DataAIFolder.Substring(0, DataAIFolder.Length - 1)
                End If
                DataAIFolder = DataAIFolder.Substring(DataAIFolder.LastIndexOf("/")).Replace("/", "").Replace(" ", "")
            End If
        End If

        Dim wcfg = String.Empty


        Dim downloadsPath = txtyourfoldertouploadfiles.Text  'or Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
        downloadsPath = downloadsPath & "\Temp\" & DataAIFolder & ".zip"
        downloadsPath = downloadsPath.Replace("\\", "\")

        'download DataAI.zip
        'Dim client As New WebClient()
        'AddHandler client.DownloadProgressChanged, AddressOf client_DownloadProgressChanged
        'AddHandler client.DownloadFileCompleted, AddressOf client_DownloadFileCompleted
        'Try
        '    progressBar1.Value = 0
        '    client.DownloadFileAsync(New Uri("https://oureports.net/OUReports/SAVEDFILES/DataAI.zip"), downloadsPath)
        'Catch ex As Exception
        '    ret = "Error: " & ex.Message
        '    MsgBox(ret)
        '    Exit Sub
        'End Try

        'check name availability
        Dim extractPath = String.Empty
        Dim iispath = "C:\inetpub\wwwroot\"
        Dim webconfigpath = iispath & DataAIFolder & "\web.config"

        'preserve previous web.config if app already exists in that directory
        If File.Exists(webconfigpath) Then
            wcfg = File.ReadAllText(webconfigpath)
            ret = ShowPreviousSetting(DataAIFolder)
        End If

        extractPath = downloadsPath.Replace(DataAIFolder & ".zip", "")   ' & "\uploads\Temp\"
        If Directory.Exists(extractPath & "DataAI") Then
            Directory.Delete(extractPath & "DataAI", True)
        End If
        'unzip in the temp directory
        Try
            ZipFile.ExtractToDirectory(downloadsPath, extractPath, True)
        Catch ex As Exception
            ret = "Error: " & ex.Message
            MsgBox(ret)
            Exit Sub

        End Try

        'rename directory from DataAI to DataAIFolder name
        If Directory.Exists(downloadsPath.Replace(".zip", "")) Then
            Directory.Delete(downloadsPath.Replace(".zip", ""), True)
        End If
        Directory.Move(extractPath & "DataAI", downloadsPath.Replace(".zip", ""))

        'copy into iispath
        If Directory.Exists(iispath & DataAIFolder) Then
            Directory.Delete(iispath & DataAIFolder, True)
        End If
        Directory.Move(downloadsPath.Replace(".zip", ""), iispath & DataAIFolder)

        'make extractPath as IIS application
        Try
            Dim serverManager As New ServerManager
            Dim site = serverManager.Sites("Default Web Site")

            Dim appPath = "/" & DataAIFolder
            Dim physicalPath = iispath & DataAIFolder

            Dim app = site.Applications.Add(appPath, physicalPath)
            app.ApplicationPoolName = "DefaultAppPool"

            serverManager.CommitChanges()

        Catch ex As Exception
            'ret = "Error: " & ex.Message
            'MsgBox(ret)
            'Exit Sub
        End Try


        ''check name availability
        'Dim iispath As String = "C:\inetpub\wwwroot\"
        'If Directory.Exists(iispath & DataAIFolder) Then 'app already exists in that directory
        '    txtyoururlDataAI.BackColor = Color.Pink
        '    MsgBox("This name is not available: " & DataAIFolder & ". Use a different one.")
        '    Exit Sub
        'Else
        '    txtyoururlDataAI.BackColor = Color.White
        'End If

        ''download dataAI.zip
        'Dim downloadsPath As String = txtyourfoldertouploadfiles.Text  'or Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
        'downloadsPath = downloadsPath & "\Temp\" & DataAIFolder & ".zip"
        'downloadsPath = downloadsPath.Replace("\\", "\")
        'Try
        '    client.DownloadFile("https://oureports.net/OUReports/SAVEDFILES/DataAI.zip", downloadsPath)
        'Catch ex As Exception
        '    ret = "Error: " & ex.Message
        '    MsgBox(ret)
        '    Exit Sub
        'End Try

        ''unzip
        'Dim extractPath As String = downloadsPath.Replace(DataAIFolder & ".zip", "")  '"C:\inetpub\wwwroot\"
        'Try
        '    ZipFile.ExtractToDirectory(downloadsPath, extractPath, True)
        'Catch ex As Exception
        '    ret = "Error: " & ex.Message
        '    MsgBox(ret)
        '    Exit Sub
        'End Try

        'Directory.Move(extractPath & "DataAI", iispath & DataAIFolder)

        ''make extractPath as IIS application
        'Try
        '    Dim serverManager As New ServerManager()
        '    Dim site = serverManager.Sites("Default Web Site")

        '    Dim appPath As String = "/" & DataAIFolder
        '    Dim physicalPath As String = "C:\inetpub\wwwroot\" & DataAIFolder

        '    Dim app = site.Applications.Add(appPath, physicalPath)
        '    app.ApplicationPoolName = "DefaultAppPool"

        '    serverManager.CommitChanges()

        'Catch ex As Exception
        '    'ret = "Error: " & ex.Message
        '    'MsgBox(ret)
        '    'Exit Sub
        'End Try

        Dim folderPath = iispath & DataAIFolder & "\KLMS"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = iispath & DataAIFolder & "\RDLFILES"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = iispath & DataAIFolder & "\Temp"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = iispath & DataAIFolder & "\XSDFILES"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = iispath & DataAIFolder & "\SAVEDFILES"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If

        ''restore previous setting in web.config
        'If wcfg.Trim <> "" Then
        '    File.WriteAllText(webconfigpath, wcfg)
        'End If

        MsgBox("Download and setting of the web site has been completed. Continue with configuration.")
        'update web.config
        btnUpdateWebConfig_Click(sender, e)

        If ret.Trim = "" AndAlso errors = "" Then
            Dim yoururl = txtyoururlDataAI.Text.Trim
            Process.Start(New ProcessStartInfo With {
                 .FileName = yoururl & "/Default.aspx",
                 .UseShellExecute = True
            })
            MsgBox("Installation/Update of the web site has been completed. Previous configuration of existing web site has been preserved. Continue with registration of teams, topics, and users on Default page at " & txtyoururlDataAI.Text)
        End If

    End Sub

    Private Sub LinkLabel1_Click(sender As Object, e As EventArgs) Handles LinkLabel1.Click
        Process.Start(New ProcessStartInfo With {
        .FileName = "https://oureports.net/OUReports/ContactUs.aspx",
        .UseShellExecute = True
    })
    End Sub
    Private Sub LinkLabel2_Click(sender As Object, e As EventArgs) Handles LinkLabel2.Click
        Process.Start(New ProcessStartInfo With {
        .FileName = cleanTextShort(txtyoururlDataAI.Text) & "/Default.aspx",
        .UseShellExecute = True
    })
    End Sub
    Private Sub LinkLabel3_Click(sender As Object, e As EventArgs) Handles LinkLabel3.Click
        Process.Start(New ProcessStartInfo With {
        .FileName = "https://platform.openai.com/",
        .UseShellExecute = True
    })
    End Sub
    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Process.Start(New ProcessStartInfo With {
        .FileName = "https://dev.mysql.com/",
        .UseShellExecute = True
    })
    End Sub
    Private Sub client_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles client.DownloadProgressChanged
        progressBar1.Value = e.ProgressPercentage
    End Sub
    Private Sub client_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles client.DownloadFileCompleted
        MessageBox.Show("Download complete. Starting the installation.")
        Install(sender, e)
    End Sub
    Public Function cleanTextShort(ByVal strText As String) As String
        'TODO more!!
        If strText Is Nothing OrElse strText = "" Then
            Return ""
        End If
        strText = strText.Replace("<%", "***")
        strText = strText.Replace("%>", "***")
        strText = strText.Replace("</", "***")
        strText = strText.Replace("/>", "***")
        strText = strText.Replace("'", "***")
        strText = strText.Replace("<>", "!=")
        strText = strText.Replace("<", "***less than***")
        strText = strText.Replace(">", "***more than***")
        strText = strText.Replace("%", " ")
        strText = strText.Replace(Chr(39), " ")

        Return strText.Trim
    End Function

    Private Sub btnUpdateWebConfig_Click(sender As Object, e As EventArgs) Handles btnUpdateWebConfig.Click
        Dim ret As String = String.Empty
        txtyoururlDataAI.Text = cleanTextShort(txtyoururlDataAI.Text)
        If txtyoururlDataAI.Text.Trim = "" Then
            txtyoururlDataAI.BackColor = Color.Pink
            MsgBox("Wrong values in textboxes")
            Exit Sub
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If
        txtyourfoldertouploadfiles.Text = cleanTextShort(txtyourfoldertouploadfiles.Text)
        If txtyourfoldertouploadfiles.Text.Trim = "" Then
            txtyourfoldertouploadfiles.BackColor = Color.Pink
            MsgBox("Wrong values in textboxes")
            Exit Sub
        Else
            txtyourfoldertouploadfiles.BackColor = Color.White
        End If
        'DataAIFolder
        If txtyoururlDataAI.Text.Trim <> "" AndAlso txtyoururlDataAI.Text.Trim <> "[your web url for DataAI]" Then
            DataAIFolder = txtyoururlDataAI.Text.Trim
            If DataAIFolder.Trim <> "" AndAlso DataAIFolder <> "[your web url for DataAI]" Then
                If DataAIFolder.LastIndexOf("/") = DataAIFolder.Length - 1 Then
                    DataAIFolder = DataAIFolder.Substring(0, DataAIFolder.Length - 1)
                End If
                DataAIFolder = DataAIFolder.Substring(DataAIFolder.LastIndexOf("/")).Replace("/", "").Replace(" ", "")
            End If
        End If

        'check entered values in textboxes
        If txtOperationalDatabaseServer.Text.Trim = "" OrElse cleanTextShort(txtOperationalDatabaseServer.Text) <> txtOperationalDatabaseServer.Text Then
            txtOperationalDatabaseServer.BackColor = Color.Pink
            txtOperationalDatabaseServer.Text = cleanTextShort(txtOperationalDatabaseServer.Text)
            ret = "Wrong values in textboxes"
        Else
            txtOperationalDatabaseServer.BackColor = Color.White
        End If
        If txtOperationalDatabaseName.Text.Trim = "" OrElse cleanTextShort(txtOperationalDatabaseName.Text) <> txtOperationalDatabaseName.Text Then
            txtOperationalDatabaseName.BackColor = Color.Pink
            txtOperationalDatabaseName.Text = cleanTextShort(txtOperationalDatabaseName.Text)
            ret = "Wrong values in textboxes"
        Else
            txtOperationalDatabaseName.BackColor = Color.White
        End If
        If txtoperationaldatabaseuserID.Text.Trim = "" OrElse cleanTextShort(txtoperationaldatabaseuserID.Text) <> txtoperationaldatabaseuserID.Text Then
            txtoperationaldatabaseuserID.BackColor = Color.Pink
            txtoperationaldatabaseuserID.Text = cleanTextShort(txtoperationaldatabaseuserID.Text)
            ret = "Wrong values in textboxes"
        Else
            txtoperationaldatabaseuserID.BackColor = Color.White
        End If
        If txtoperationaldatabasepassword.Text.Trim = "" OrElse cleanTextShort(txtoperationaldatabasepassword.Text) <> txtoperationaldatabasepassword.Text Then
            txtoperationaldatabasepassword.BackColor = Color.Pink
            txtoperationaldatabasepassword.Text = cleanTextShort(txtoperationaldatabasepassword.Text)
            ret = "Wrong values in textboxes"
        Else
            txtoperationaldatabasepassword.BackColor = Color.White
        End If
        If txtoperationaldatabaseprovider.Text.Trim = "" OrElse cleanTextShort(txtoperationaldatabaseprovider.Text) <> txtoperationaldatabaseprovider.Text Then
            txtoperationaldatabaseprovider.BackColor = Color.Pink
            txtoperationaldatabaseprovider.Text = cleanTextShort(txtoperationaldatabaseprovider.Text)
            ret = "Wrong values in textboxes"
        Else
            txtoperationaldatabaseprovider.BackColor = Color.White
        End If
        If Not ("MySql.Data.MySqlClient,System.Data.SqlClient,Npgsql,Oracle.ManagedData.Client,InterSystems.Data.IRISClient,InterSystems.Data.CacheClient").Contains(txtoperationaldatabaseprovider.Text.Trim) Then
            txtoperationaldatabaseprovider.BackColor = Color.Pink
            ret = "Wrong values in textboxes"
        End If
        If cleanTextShort(txtoperationaldatabaseport.Text) <> txtoperationaldatabaseport.Text Then
            txtoperationaldatabaseport.Text = cleanTextShort(txtoperationaldatabaseport.Text)
            txtoperationaldatabaseport.BackColor = Color.Pink
            ret = "Wrong values in textboxes"
        Else
            txtoperationaldatabaseport.BackColor = Color.White
        End If
        If cleanTextShort(txtsystemdatabasepassword.Text) <> txtsystemdatabasepassword.Text Then
            txtsystemdatabasepassword.BackColor = Color.Pink
            txtsystemdatabasepassword.Text = cleanTextShort(txtsystemdatabasepassword.Text)
            ret = "Wrong values in textboxes"
        Else
            txtoperationaldatabaseport.BackColor = Color.White
        End If

        If txtyourdataserver.Text.Trim = "" OrElse cleanTextShort(txtyourdataserver.Text) <> txtyourdataserver.Text Then
            txtyourdataserver.BackColor = Color.Pink
            txtyourdataserver.Text = cleanTextShort(txtyourdataserver.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyourdataserver.BackColor = Color.White
        End If
        If txtyourDatabaseName.Text.Trim = "" OrElse cleanTextShort(txtyourDatabaseName.Text) <> txtyourDatabaseName.Text Then
            txtyourDatabaseName.BackColor = Color.Pink
            txtyourDatabaseName.Text = cleanTextShort(txtyourDatabaseName.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyourDatabaseName.BackColor = Color.White
        End If
        If txtyourdatabaseuserID.Text.Trim = "" OrElse cleanTextShort(txtyourdatabaseuserID.Text) <> txtyourdatabaseuserID.Text Then
            txtyourdatabaseuserID.BackColor = Color.Pink
            txtyourdatabaseuserID.Text = cleanTextShort(txtyourdatabaseuserID.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyourdatabaseuserID.BackColor = Color.White
        End If
        If txtyourdatabasepassword.Text.Trim = "" OrElse cleanTextShort(txtyourdatabasepassword.Text) <> txtyourdatabasepassword.Text Then
            txtyourdatabasepassword.BackColor = Color.Pink
            txtyourdatabasepassword.Text = cleanTextShort(txtyourdatabasepassword.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyourdatabasepassword.BackColor = Color.White
        End If
        If txtyourdatabaseprovider.Text.Trim = "" OrElse cleanTextShort(txtyourdatabaseprovider.Text) <> txtyourdatabaseprovider.Text Then
            txtyourdatabaseprovider.BackColor = Color.Pink
            txtyourdatabaseprovider.Text = cleanTextShort(txtyourdatabaseprovider.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyourdatabaseprovider.BackColor = Color.White
        End If
        If Not ("MySql.Data.MySqlClient,System.Data.SqlClient,Npgsql,Oracle.ManagedData.Client,InterSystems.Data.IRISClient,InterSystems.Data.CacheClient").Contains(txtyourdatabaseprovider.Text.Trim) Then
            txtyourdatabaseprovider.BackColor = Color.Pink
            ret = "Wrong values in textboxes"
        End If
        If cleanTextShort(txtyourdatabaseport.Text) <> txtyourdatabaseport.Text Then
            txtyourdatabaseport.BackColor = Color.Pink
            txtyourdatabaseport.Text = cleanTextShort(txtyourdatabaseport.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyourdatabaseport.BackColor = Color.White
        End If

        If txtyoursmtpemail.Text.Trim = "" OrElse cleanTextShort(txtyoursmtpemail.Text) <> txtyoursmtpemail.Text Then
            txtyoursmtpemail.BackColor = Color.Pink
            txtyoursmtpemail.Text = cleanTextShort(txtyoursmtpemail.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyoursmtpemail.BackColor = Color.White
        End If
        If txtyoursmtpemailpassword.Text.Trim = "" OrElse cleanTextShort(txtyoursmtpemailpassword.Text) <> txtyoursmtpemailpassword.Text Then
            txtyoursmtpemailpassword.BackColor = Color.Pink
            txtyoursmtpemailpassword.Text = cleanTextShort(txtyoursmtpemailpassword.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyoursmtpemailpassword.BackColor = Color.White
        End If

        If txtyoururlDataAI.Text.Trim = "" OrElse cleanTextShort(txtyoururlDataAI.Text) <> txtyoururlDataAI.Text.Trim Then
            txtyoururlDataAI.BackColor = Color.Pink
            txtyoururlDataAI.Text = cleanTextShort(txtyoururlDataAI.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If
        txtyourwebsitetitle.Text = cleanTextShort(txtyourwebsitetitle.Text)
        If txtyourwebsitetitle.Text.Trim = "" Then
            txtyourwebsitetitle.BackColor = Color.Pink
            txtyourwebsitetitle.Text = cleanTextShort(txtyourwebsitetitle.Text)
            'ret = "Wrong values in textboxes"
        Else
            txtyourwebsitetitle.BackColor = Color.White
        End If
        If txtyoursupportemail.Text.Trim = "" OrElse cleanTextShort(txtyoursupportemail.Text) <> txtyoursupportemail.Text Then
            txtyoursupportemail.BackColor = Color.Pink
            txtyoursupportemail.Text = cleanTextShort(txtyoursupportemail.Text)
            ret = "Wrong values in textboxes"
        Else
            txtyoursupportemail.BackColor = Color.White
        End If
        'If txtyourfoldertouploadfiles.Text.Trim = "" OrElse cleanTextShort(txtyourfoldertouploadfiles.Text) <> txtyourfoldertouploadfiles.Text Then
        '    txtyourfoldertouploadfiles.BackColor = Color.Pink
        '    txtyourfoldertouploadfiles.Text = cleanTextShort(txtyourfoldertouploadfiles.Text)
        '    ret = "Wrong values in textboxes"
        'Else
        '    txtyourfoldertouploadfiles.BackColor = Color.White
        'End If
        If cleanTextShort(txtyourgooglemapkey.Text) <> txtyourgooglemapkey.Text Then
            txtyourgooglemapkey.BackColor = Color.Pink
            txtyourgooglemapkey.Text = cleanTextShort(txtyourgooglemapkey.Text)
            'ret = "Wrong values in textboxes"
        Else
            txtyourgooglemapkey.BackColor = Color.White
        End If

        If txtyourOpenAIkey.Text.Trim = "" OrElse cleanTextShort(txtyourOpenAIkey.Text) <> txtyourOpenAIkey.Text Then
            txtyourOpenAIkey.BackColor = Color.Pink
            txtyourOpenAIkey.Text = cleanTextShort(txtyourOpenAIkey.Text)
            'ret = "Wrong values in textboxes"
        Else
            txtyourOpenAIkey.BackColor = Color.White
        End If
        If txtyourOpenAImodel.Text.Trim = "" OrElse cleanTextShort(txtyourOpenAImodel.Text) <> txtyourOpenAImodel.Text Then
            txtyourOpenAImodel.BackColor = Color.Pink
            txtyourOpenAImodel.Text = cleanTextShort(txtyourOpenAImodel.Text)
            'ret = "Wrong values in textboxes"
        Else
            txtyourOpenAImodel.BackColor = Color.White
        End If
        If txtyourmaximumtokens.Text.Trim = "" OrElse cleanTextShort(txtyourmaximumtokens.Text) <> txtyourmaximumtokens.Text Then
            txtyourmaximumtokens.BackColor = Color.Pink
            txtyourmaximumtokens.Text = cleanTextShort(txtyourmaximumtokens.Text)
            'ret = "Wrong values in textboxes"
        Else
            txtyourmaximumtokens.BackColor = Color.White
        End If
        If txtyourOpenAIorganizationcode.Text.Trim = "" OrElse cleanTextShort(txtyourOpenAIorganizationcode.Text) <> txtyourOpenAIorganizationcode.Text Then
            txtyourOpenAIorganizationcode.BackColor = Color.Pink
            txtyourOpenAIorganizationcode.Text = cleanTextShort(txtyourOpenAIorganizationcode.Text)
            'ret = "Wrong values in textboxes"
        Else
            txtyourOpenAIorganizationcode.BackColor = Color.White
        End If
        If txtyourOpenAIURL.Text.Trim = "" OrElse cleanTextShort(txtyourOpenAIURL.Text) <> txtyourOpenAIURL.Text Then
            txtyourOpenAIURL.BackColor = Color.Pink
            txtyourOpenAIURL.Text = cleanTextShort(txtyourOpenAIURL.Text)
            'ret = "Wrong values in textboxes"
        Else
            txtyourOpenAIURL.BackColor = Color.White
        End If

        If ret.Trim <> "" Then
            errors = ret
            MsgBox("Wrong values in textboxes. Enter proper values and Update Configration...")
            Exit Sub
        End If

        'update web.config
        Try
            Dim webconfigpath As String = "C:\inetpub\wwwroot\" & DataAIFolder & "\web.config"
            Dim weborgconfigpath As String = "C:\inetpub\wwwroot\" & DataAIFolder & "\webOriginal.config"
            Dim wcfg As String = String.Empty
            Dim firsttimesetting As Boolean = False
            If Not File.Exists(weborgconfigpath) Then  'write original web config
                wcfg = File.ReadAllText(webconfigpath)
                File.WriteAllText(weborgconfigpath, wcfg)
                firsttimesetting = True
            Else 'restore the original web config
                wcfg = File.ReadAllText(weborgconfigpath)
                If File.Exists(webconfigpath) Then
                    File.Delete(webconfigpath)
                End If
                File.WriteAllText(webconfigpath, wcfg)
            End If
            wcfg = File.ReadAllText(webconfigpath)
            wcfg = wcfg.Replace("[operational database server]", txtOperationalDatabaseServer.Text)
            wcfg = wcfg.Replace("[operational database name]", txtOperationalDatabaseName.Text)
            If txtoperationaldatabaseprovider.Text = "Oracle.ManagedData.Client" Then
                wcfg = wcfg.Replace("[operational data source]", txtOperationalDatabaseName.Text)
            ElseIf txtoperationaldatabaseprovider.Text.Contains("InterSystems.Data") Then
                wcfg = wcfg.Replace("[operational namespace name]", txtOperationalDatabaseName.Text)
            End If
            wcfg = wcfg.Replace("[operational database user ID]", txtoperationaldatabaseuserID.Text)
            wcfg = wcfg.Replace("[operational database password]", txtoperationaldatabasepassword.Text)
            wcfg = wcfg.Replace("[operational database provider name]", txtoperationaldatabaseprovider.Text)
            If txtoperationaldatabaseport.Text.Trim <> "" Then
                wcfg = wcfg.Replace("[operational database port if needed]", txtoperationaldatabaseport.Text)
            End If
            If txtsystemdatabasepassword.Text.Trim <> "" Then
                wcfg = wcfg.Replace("[system database password if needed]", txtsystemdatabasepassword.Text)
            End If
            wcfg = wcfg.Replace("[your data server]", txtyourdataserver.Text)
            wcfg = wcfg.Replace("[your database name]", txtyourDatabaseName.Text)
            If txtyourdatabaseprovider.Text = "Oracle.ManagedData.Client" Then
                wcfg = wcfg.Replace("[your data source]", txtyourDatabaseName.Text)
            ElseIf txtyourdatabaseprovider.Text.Contains("InterSystems.Data") Then
                wcfg = wcfg.Replace("[your namespace name]", txtyourDatabaseName.Text)
            End If
            wcfg = wcfg.Replace("[your database user ID]", txtyourdatabaseuserID.Text)
            wcfg = wcfg.Replace("[your database password]", txtyourdatabasepassword.Text)
            wcfg = wcfg.Replace("[your database provider name]", txtyourdatabaseprovider.Text)
            If txtyourdatabaseport.Text.Trim <> "" Then
                wcfg = wcfg.Replace("[your database port if needed]", txtyourdatabaseport.Text)
            End If
            wcfg = wcfg.Replace("[your smtp email]", txtyoursmtpemail.Text)
            wcfg = wcfg.Replace("[password for your smtp email]", txtyoursmtpemailpassword.Text)

            wcfg = wcfg.Replace("[your web url for DataAI]", txtyoururlDataAI.Text)
            wcfg = wcfg.Replace("[your web site title]", txtyourwebsitetitle.Text)
            wcfg = wcfg.Replace("[your support email]", txtyoursupportemail.Text)
            wcfg = wcfg.Replace("[your folder to upload files]", txtyourfoldertouploadfiles.Text)
            If txtyourgooglemapkey.Text.Trim <> "" Then
                wcfg = wcfg.Replace("[your google map key]", txtyourgooglemapkey.Text)
            End If
            wcfg = wcfg.Replace("[your OpenAI key]", txtyourOpenAIkey.Text)
            wcfg = wcfg.Replace("[OpenAI model]", txtyourOpenAImodel.Text)
            wcfg = wcfg.Replace("[OpenAI maximum tokens limit]", txtyourmaximumtokens.Text)
            wcfg = wcfg.Replace("[your OpenAI organization code]", txtyourOpenAIorganizationcode.Text)

            File.WriteAllText(webconfigpath, wcfg)

            'uncomment proper connecton strings for proper provider
            Dim sr() As String = File.ReadAllLines(webconfigpath)
            For i = 0 To sr.Length - 1
                If txtoperationaldatabaseprovider.Text = "System.Data.SqlClient" Then
                    If sr(i).Contains("//Sql Server") Then
                        sr(i) = " //Sql Server -->"
                    End If
                    If sr(i).Contains("//Postgre") Then
                        sr(i) = "<!-- //Postgre"
                    End If
                ElseIf txtoperationaldatabaseprovider.Text = "MySql.Data.MySqlClient" Then
                    If sr(i).Contains("//MySql") Then
                        sr(i) = " //MySql -->"
                    End If
                    If sr(i).Contains("//Sql Server") Then
                        sr(i) = "<!-- //Sql Server"
                    End If
                ElseIf txtoperationaldatabaseprovider.Text = "Npgsql" Then  'Postgre
                    If sr(i).Contains("//Postgre") Then
                        sr(i) = " //Postgre -->"
                    End If
                    If sr(i).Contains("//Oracle") Then
                        sr(i) = "<!-- //Oracle"
                    End If
                ElseIf txtoperationaldatabaseprovider.Text = "Oracle.ManagedData.Client" Then
                    If sr(i).Contains("//Oracle") Then
                        sr(i) = " //Oracle -->"
                    End If
                    If sr(i).Contains("//Intersystems IRIS") Then
                        sr(i) = "<!-- //Intersystems IRIS"
                    End If
                ElseIf txtoperationaldatabaseprovider.Text = "InterSystems.Data.IRISClient" Then
                    If sr(i).Contains("//Intersystems IRIS") Then
                        sr(i) = " //Intersystems IRIS -->"
                    End If
                    If sr(i).Contains("//Intersystems Cache") Then
                        sr(i) = "<!-- //Intersystems Cache"
                    End If
                ElseIf txtoperationaldatabaseprovider.Text = "MySql.Data.MySqlClient" Then
                    If sr(i).Contains("//Intersystems Cache") Then
                        sr(i) = " //Intersystems Cache -->"
                    End If
                    If sr(i).Contains("//end") Then
                        sr(i) = "<!-- //end"
                    End If
                End If
            Next

            'write the setting values to the end of web.config
            Dim n As Integer = sr.Length
            ReDim Preserve sr(n + 30)

            sr(n) = "  <!--  "
            sr(n + 1) = "//Previous setting:"
            sr(n + 2) = "//[operational database server] " & txtOperationalDatabaseServer.Text
            sr(n + 3) = "//[operational database name] " & txtOperationalDatabaseName.Text
            sr(n + 4) = "//[operational database user ID] " & txtoperationaldatabaseuserID.Text
            sr(n + 5) = "//[operational database password] " & txtoperationaldatabasepassword.Text
            sr(n + 6) = "//[operational database provider name] " & txtoperationaldatabaseprovider.Text
            sr(n + 7) = "//[operational database port if needed] " & txtoperationaldatabaseport.Text
            sr(n + 8) = "//[system database password if needed] " & txtsystemdatabasepassword.Text

            sr(n + 9) = "//[your data server] " & txtyourdataserver.Text
            sr(n + 10) = "//[your database name] " & txtyourDatabaseName.Text
            sr(n + 11) = "//[your database user ID] " & txtyourdatabaseuserID.Text
            sr(n + 12) = "//[your database password] " & txtyourdatabasepassword.Text
            sr(n + 13) = "//[your database provider name] " & txtyourdatabaseprovider.Text
            sr(n + 14) = "//[your database port if needed] " & txtyourdatabaseport.Text

            sr(n + 15) = "//[your smtp email] " & txtyoursmtpemail.Text
            sr(n + 16) = "//[password for your smtp email] " & txtyoursmtpemailpassword.Text

            sr(n + 17) = "//[your web url for DataAI] " & txtyoururlDataAI.Text
            sr(n + 18) = "//[your web site title] " & txtyourwebsitetitle.Text
            sr(n + 19) = "//[your support email] " & txtyoursupportemail.Text
            sr(n + 20) = "//[your folder to upload files] " & txtyourfoldertouploadfiles.Text
            sr(n + 21) = "//[your google map key] " & txtyourgooglemapkey.Text

            sr(n + 22) = "//[your OpenAI key] " & txtyourOpenAIkey.Text
            sr(n + 23) = "//[OpenAI model] " & txtyourOpenAImodel.Text
            sr(n + 24) = "//[OpenAI maximum tokens limit] " & txtyourmaximumtokens.Text
            sr(n + 25) = "//[your OpenAI organization code] " & txtyourOpenAIorganizationcode.Text

            sr(n + 26) = "   -->   "

            'write in file
            File.WriteAllLines(webconfigpath, sr)

        Catch ex As Exception
            ret = "Error: " & ex.Message
            MsgBox(ret)
            Exit Sub
        End Try

        'If ret.Trim = "" Then
        '    'Response.Redirect("Default.aspx")
        '    Dim yoururl As String = txtyoururlDataAI.Text.Trim
        '    Process.Start(New ProcessStartInfo With {
        '         .FileName = yoururl & "/Default.aspx",
        '         .UseShellExecute = True
        '    })
        'End If

        MsgBox("Configuration of the web site has been updated.")
    End Sub

    Private Sub btnRestoreDefault_Click(sender As Object, e As EventArgs) Handles btnRestoreDefault.Click
        If txtyoururlDataAI.Text.Trim = "" OrElse cleanTextShort(txtyoururlDataAI.Text) <> txtyoururlDataAI.Text.Trim Then
            txtyoururlDataAI.BackColor = Color.Pink
            MsgBox("Wrong values in textboxes")
            Exit Sub
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If
        If txtyoururlDataAI.Text.Trim <> "" AndAlso txtyoururlDataAI.Text.Trim <> "[your web url for DataAI]" Then
            DataAIFolder = txtyoururlDataAI.Text.Trim
            If DataAIFolder.Trim <> "" AndAlso DataAIFolder <> "[your web url for DataAI]" Then
                If DataAIFolder.LastIndexOf("/") = DataAIFolder.Length - 1 Then
                    DataAIFolder = DataAIFolder.Substring(0, DataAIFolder.Length - 1)
                End If
                DataAIFolder = DataAIFolder.Substring(DataAIFolder.LastIndexOf("/")).Replace("/", "").Replace(" ", "")
            End If
        End If
        If Not Directory.Exists("C:\inetpub\wwwroot\" & DataAIFolder) Then
            'MsgBox("Directory " & TaskListFolder & " does not exist. Download and Install It first")
            txtyoururlDataAI.BackColor = Color.Pink
            Exit Sub
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If
        Dim webconfigpath As String = "C:\inetpub\wwwroot\" & DataAIFolder & "\web.config"
        Dim weborgconfigpath As String = "C:\inetpub\wwwroot\" & DataAIFolder & "\webOriginal.config"
        If Not File.Exists(weborgconfigpath) Then  'web.config is original
            Exit Sub
        End If
        Dim wcfg As String = File.ReadAllText(weborgconfigpath)
        If File.Exists(webconfigpath) Then
            File.Delete(webconfigpath)
        End If
        File.WriteAllText(webconfigpath, wcfg)
        MsgBox("Default setting of the web site has been restored.")
    End Sub

    Private Sub btnShowSetting_Click(sender As Object, e As EventArgs) Handles btnShowSetting.Click
        If txtyoururlDataAI.Text.Trim = "" OrElse cleanTextShort(txtyoururlDataAI.Text) <> txtyoururlDataAI.Text.Trim Then
            txtyoururlDataAI.BackColor = Color.Pink
            MsgBox("Wrong values in textboxes")
            Exit Sub
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If
        If txtyoururlDataAI.Text.Trim <> "" AndAlso txtyoururlDataAI.Text.Trim <> "[your web url for DataAI]" Then
            DataAIFolder = txtyoururlDataAI.Text.Trim
            If DataAIFolder.Trim <> "" AndAlso DataAIFolder <> "[your web url for DataAI]" Then
                If DataAIFolder.LastIndexOf("/") = DataAIFolder.Length - 1 Then
                    DataAIFolder = DataAIFolder.Substring(0, DataAIFolder.Length - 1)
                End If
                DataAIFolder = DataAIFolder.Substring(DataAIFolder.LastIndexOf("/")).Replace("/", "").Replace(" ", "")
            End If
        End If

        'restore previous setting
        Dim ret As String = ShowPreviousSetting(DataAIFolder)
        MsgBox("Current setting of web site shows up.")
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim ret = String.Empty
        If txtyoururlDataAI.Text.Trim = "" OrElse cleanTextShort(txtyoururlDataAI.Text) <> txtyoururlDataAI.Text.Trim Then
            txtyoururlDataAI.BackColor = Color.Pink
            MsgBox("Wrong values in textboxes")
            Exit Sub
        Else
            txtyoururlDataAI.BackColor = Color.White
        End If
        'If txtyourfoldertouploadfiles.Text.Trim = "" OrElse cleanTextShort(txtyourfoldertouploadfiles.Text) <> txtyourfoldertouploadfiles.Text Then
        '    txtyourfoldertouploadfiles.BackColor = Color.Pink
        '    MsgBox("Wrong values in textboxes")
        '    Exit Sub
        'Else
        '    txtyourfoldertouploadfiles.BackColor = Color.White
        'End If
        If txtyoururlDataAI.Text.Trim <> "" AndAlso txtyoururlDataAI.Text.Trim <> "[your web url for DataAI]" Then
            DataAIFolder = txtyoururlDataAI.Text.Trim
            If DataAIFolder.Trim <> "" AndAlso DataAIFolder <> "[your web url for DataAI]" Then
                If DataAIFolder.LastIndexOf("/") = DataAIFolder.Length - 1 Then
                    DataAIFolder = DataAIFolder.Substring(0, DataAIFolder.Length - 1)
                End If
                DataAIFolder = DataAIFolder.Substring(DataAIFolder.LastIndexOf("/")).Replace("/", "").Replace(" ", "")
            End If
        End If

        'download DataAI.zip
        'downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
        If Not Directory.Exists(downloadsPath) Then
            Directory.CreateDirectory(downloadsPath)
        End If
        If Not Directory.Exists(downloadsPath & "\Temp\") Then
            Directory.CreateDirectory(downloadsPath & "\Temp\")
        End If
        Dim downloadsPathToZip As String = downloadsPath & "\Temp\" & DataAIFolder & ".zip"
        downloadsPathToZip = downloadsPathToZip.Replace(" ", "").Replace("\\", "\")

        Dim client As New WebClient()
        AddHandler client.DownloadProgressChanged, AddressOf client_DownloadProgressChanged
        AddHandler client.DownloadFileCompleted, AddressOf client_DownloadFileCompleted
        Try
            progressBar1.Value = 0
            client.DownloadFileAsync(New Uri("https://oureports.net/OUReports/SAVEDFILES/DataAI.zip"), downloadsPathToZip)
        Catch ex As Exception
            ret = "Error: " & ex.Message
            MsgBox("Error: " & ex.Message)
            Exit Sub
        End Try
    End Sub
    Private Sub Install(sender As Object, e As EventArgs)
        Dim ret = String.Empty
        Dim wcfg = String.Empty

        'Dim downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")   'or txtyourfoldertouploadfiles.Text
        Dim downloadsPathToZip = downloadsPath & "\Temp\" & DataAIFolder & ".zip"
        downloadsPathToZip = downloadsPathToZip.Replace(" ", "").Replace("\\", "\")

        'check name availability
        Dim extractPath = String.Empty
        extractPath = downloadsPathToZip.Replace(DataAIFolder & ".zip", "")   ' & "\uploads\Temp\"

        Dim iispath = "C:\inetpub\wwwroot\"
        Dim webconfigpath = iispath & DataAIFolder & "\web.config"

        'preserve previous web.config if app already exists in that directory
        If File.Exists(webconfigpath) Then
            wcfg = File.ReadAllText(webconfigpath)
            ret = ShowPreviousSetting(DataAIFolder)
        End If


        If Directory.Exists(extractPath & "DataAI") AndAlso DataAIFolder = "DataAI" Then
            Directory.Delete(extractPath & "DataAI", True)
        End If
        'unzip in the temp directory
        Try
            ZipFile.ExtractToDirectory(downloadsPathToZip, extractPath, True)
        Catch ex As Exception
            ret = "Error: " & ex.Message
            MsgBox(ret)
            Exit Sub

        End Try

        'rename directory from DataAI to DataAIFolder name
        If Directory.Exists(downloadsPathToZip.Replace(".zip", "")) AndAlso DataAIFolder <> "DataAI" Then
            Directory.Delete(downloadsPathToZip.Replace(".zip", ""), True)

        End If
        If Not Directory.Exists(downloadsPathToZip.Replace(".zip", "")) AndAlso DataAIFolder <> "DataAI" Then
            Directory.Move(extractPath & "DataAI", downloadsPathToZip.Replace(".zip", ""))
        End If

        'copy into iispath
        If Directory.Exists(iispath & DataAIFolder) Then
            Directory.Delete(iispath & DataAIFolder, True)
        End If
        Directory.Move(downloadsPathToZip.Replace(".zip", ""), iispath & DataAIFolder)

        'make extractPath as IIS application
        Try
            Dim serverManager As New ServerManager
            Dim site = serverManager.Sites("Default Web Site")

            Dim appPath = "/" & DataAIFolder
            Dim physicalPath = iispath & DataAIFolder

            If Not Directory.Exists(physicalPath) Then
                Directory.CreateDirectory(physicalPath)
            End If

            Try
                Dim app = site.Applications.Add(appPath, physicalPath)
                app.ApplicationPoolName = "DefaultAppPool"
            Catch ex As Exception
                ret = ex.Message
            End Try

            ' Ensure the app pool uses ApplicationPoolIdentity (and loads user profile)
            Dim pool = serverManager.ApplicationPools("DefaultAppPool")
            If pool Is Nothing Then Throw New Exception("App pool 'DefaultAppPool' not found.")
            pool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity
            pool.ProcessModel.LoadUserProfile = True

            ' === AUTHENTICATION at the application scope ===
            Dim location As String = site.Name & "/" & DataAIFolder   ' e.g. "Default Web Site/YourApp"
            Dim cfg = serverManager.GetApplicationHostConfiguration()

            ' Enable Anonymous and use app-pool identity (userName = "")
            Dim anon = cfg.GetSection("system.webServer/security/authentication/anonymousAuthentication", location)
            anon("enabled") = True
            anon("userName") = ""      ' empty => use Application Pool identity

            ' (Optional) turn off other auth methods here
            Dim win = cfg.GetSection("system.webServer/security/authentication/windowsAuthentication", location)
            win("enabled") = False
            Dim basic = cfg.GetSection("system.webServer/security/authentication/basicAuthentication", location)
            basic("enabled") = False

            ' (Optional) IIS authorization: allow everyone at this app
            Dim authz = cfg.GetSection("system.webServer/security/authorization", location)
            Dim rules = authz.GetCollection()
            rules.Clear()
            Dim allow = rules.CreateElement("add")
            allow("accessType") = "Allow"
            allow("users") = "*"
            rules.Add(allow)


            serverManager.CommitChanges()


            ' === FILESYSTEM ACL (recommended): grant RX to the app pool account ===
            ' Note: also requires admin; skip if you set ACLs another way.
            Dim di = New DirectoryInfo(physicalPath)
            Dim ds = di.GetAccessControl()
            ds.AddAccessRule(New FileSystemAccessRule(
                "IIS AppPool\DefaultAppPool",
                FileSystemRights.ReadAndExecute,
                InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow))
            di.SetAccessControl(ds)

        Catch ex As Exception
            ret = "Error: " & ex.Message
            'MsgBox(ret)
            'Exit Sub

        End Try

        Dim folderPath = downloadsPath & "\KMLS"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = downloadsPath & "\RDLFILES"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = downloadsPath & "\Temp"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = downloadsPath & "\XSDFILES"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = downloadsPath & "\ImageFiles"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        folderPath = downloadsPath & "\Controls\ReportDesignerImages"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
            'download the standard reportdesignerimage.ico file from the OUReports to this directory folderPath
            Dim client As New WebClient()
            client.DownloadFile(New Uri("https://oureports.net/OUReports/Controls/ReportDesignerImages/reportdesignerimage.ico"), folderPath & "\reportdesignerimage.ico")
        End If

        MsgBox("Download and setting of the web site has been completed. Continue with configuration.")
        'update web.config
        btnUpdateWebConfig_Click(sender, e)

        If ret.Trim = "" AndAlso errors = "" Then
            Dim yoururl = txtyoururlDataAI.Text.Trim
            Process.Start(New ProcessStartInfo With {
                 .FileName = yoururl & "/Default.aspx",
                 .UseShellExecute = True
            })
            MsgBox("Installation/Update of the web site has been completed. Previous configuration of existing web site has been preserved. Connect to DataAI openning Default.aspx page at " & txtyoururlDataAI.Text)
        End If

    End Sub

    Private Sub txtoperationaldatabaseprovider_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtoperationaldatabaseprovider.SelectedIndexChanged
        '"MySql.Data.MySqlClient,System.Data.SqlClient,Npgsql,Oracle.ManagedData.Client,InterSystems.Data.IRISClient,InterSystems.Data.CacheClient")
        If (txtoperationaldatabaseprovider.SelectedItem = "Npgsql" OrElse txtoperationaldatabaseprovider.SelectedItem.ToString.StartsWith("InterSystems.Data.")) AndAlso txtoperationaldatabaseport.Text.Trim = "" Then
            txtoperationaldatabaseport.BackColor = Color.Pink
            txtoperationaldatabaseport.Text = cleanTextShort(txtoperationaldatabaseport.Text)
            'errors = "Wrong values in textboxes"
        Else
            txtoperationaldatabaseport.BackColor = Color.White
        End If
        If (txtoperationaldatabaseprovider.SelectedItem = "Oracle.ManagedData.Client" OrElse txtoperationaldatabaseprovider.SelectedItem.ToString.StartsWith("InterSystems.Data.")) AndAlso txtsystemdatabasepassword.Text.Trim = "" Then
            txtsystemdatabasepassword.BackColor = Color.Pink
            txtsystemdatabasepassword.Text = cleanTextShort(txtsystemdatabasepassword.Text)
            'errors = "Wrong values in textboxes"
        Else
            txtsystemdatabasepassword.BackColor = Color.White
        End If
    End Sub
    Private Sub txtyourdatabaseprovider_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtyourdatabaseprovider.SelectedIndexChanged
        '"MySql.Data.MySqlClient,System.Data.SqlClient,Npgsql,Oracle.ManagedData.Client,InterSystems.Data.IRISClient,InterSystems.Data.CacheClient")
        If (txtyourdatabaseprovider.SelectedItem = "Npgsql" OrElse txtyourdatabaseprovider.SelectedItem.ToString.StartsWith("InterSystems.Data.")) AndAlso txtyourdatabaseport.Text.Trim = "" Then
            txtyourdatabaseport.BackColor = Color.Pink
            txtyourdatabaseport.Text = cleanTextShort(txtyourdatabaseport.Text)
            'errors = "Wrong values in textboxes"
        Else
            txtyourdatabaseport.BackColor = Color.White
        End If
        If (txtyourdatabaseprovider.SelectedItem = "Oracle.ManagedData.Client" OrElse txtyourdatabaseprovider.SelectedItem.ToString.StartsWith("InterSystems.Data.")) AndAlso txtsystemdatabasepassword.Text.Trim = "" Then
            txtsystemdatabasepassword.BackColor = Color.Pink
            txtsystemdatabasepassword.Text = cleanTextShort(txtsystemdatabasepassword.Text)
            'errors = "Wrong values in textboxes"
        Else
            txtsystemdatabasepassword.BackColor = Color.White
        End If
    End Sub

End Class