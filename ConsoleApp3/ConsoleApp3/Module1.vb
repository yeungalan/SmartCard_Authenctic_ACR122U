Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Text
Imports System


Module Module1


    Sub Main()




        '初始socket
        Dim listener As New System.Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)
        Dim ipHostInfo As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry("localhost")
        Dim ipAddress As System.Net.IPAddress = ipHostInfo.AddressList(1)
        Dim localEndPoint As New System.Net.IPEndPoint(ipAddress, 11000)
        listener.Bind(localEndPoint)
        listener.Listen(10)
        Try
            While True
                Try
                    Dim bytes() As Byte
                    Dim handler As System.Net.Sockets.Socket = listener.Accept() '建立連接請求



                    Dim data As String = Nothing
                    bytes = New Byte(1024) {}
                    Dim bytesRec As Integer = handler.Receive(bytes) '接收資料
                    If bytesRec > 0 Then
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRec)
                        Console.WriteLine(data)
                    Else
                        Exit Sub
                    End If



                    Dim proc As ProcessStartInfo = New ProcessStartInfo("java.exe")
                    Dim pr As Process
                    proc.Arguments = "-jar .\SMC.jar"
                    proc.CreateNoWindow = True
                    proc.UseShellExecute = False
                    proc.RedirectStandardInput = True
                    proc.RedirectStandardOutput = True
                    pr = Process.Start(proc)
                    pr.StandardInput.Close()
                    ' Console.WriteLine(pr.StandardOutput.ReadToEnd())
                    Dim html As String = """" & pr.StandardOutput.ReadToEnd().Split(vbCrLf)(3).Split(" ")(1).Substring(12, 16) & """"

                    Console.WriteLine(html)
                    pr.StandardOutput.Close()


                    Dim htmlHeader As String =
                    "HTTP/1.0 200 OK" & ControlChars.CrLf &
                    "Connection: Close" & ControlChars.CrLf &
                    "Access-Control-Allow-Origin: *" & ControlChars.CrLf &
                    "Server: WebServer 1.0" & ControlChars.CrLf &
                    "Content-Length: " & html.Length & ControlChars.CrLf &
                    "Content-Type: text/html" &
                    ControlChars.CrLf & ControlChars.CrLf

                    Dim headerByte() As Byte = Encoding.ASCII.GetBytes(htmlHeader)
                    handler.Send(headerByte, headerByte.Length, SocketFlags.None)

                    Dim htmlByte() As Byte = Encoding.UTF8.GetBytes(html)
                    handler.Send(htmlByte, 0, htmlByte.Length, SocketFlags.None)


                    handler.Shutdown(Net.Sockets.SocketShutdown.Both)
                    handler.Close()
                Catch ex As Exception
                    Console.WriteLine(ex.Message)

                End Try
            End While
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Console.ReadKey()

    End Sub


End Module