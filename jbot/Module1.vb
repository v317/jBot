'Alt custom Twitch bot v.0.2
'Easy to customize!
'config.txt for server and bot settings
'permissions.txt for usernames to have access to the bot!
'---------------------
Imports System.IO
Imports System.Net.Sockets

Module Module1
    Sub Main()
        Console.Title = "jbot v.0.2"

        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("Welcome to jBot by Alt")
        Console.ResetColor()
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("----------------------------")
        Console.ResetColor()
        ' Read the IRC server and port from the config file
        Dim server As String = ReadConfigValue("server")
        Dim port As Integer = Convert.ToInt32(ReadConfigValue("port"))
        ' Read the Twitch channel and bot name from the config file
        Dim channel As String = ReadConfigValue("channel")
        Dim botName As String = ReadConfigValue("botName")
        ' Read the OAuth token for the bot from the config file
        Dim oauth As String = ReadConfigValue("oauth")
        ' Initialize the death count to 0
        Dim deathCount As Integer = 0
        Dim setCount As Integer = 0

        ' Connect to the IRC server
        Dim client As New TcpClient(server, port)
        Dim stream As NetworkStream = client.GetStream()
        Dim reader As New StreamReader(stream)
        Dim writer As New StreamWriter(stream)

        ' Send the login and channel join commands
        writer.WriteLine("PASS " & oauth)
        writer.WriteLine("NICK " & botName)
        writer.WriteLine("USER " & botName & " 8 * :" & botName)
        writer.WriteLine("JOIN " & channel)
        writer.Flush()

        ' Print a message when the bot has connected
        Console.WriteLine("Connected to " & server & " as " & botName)

        ' Keep reading messages from the IRC server
        Dim message As String
        Do
            message = reader.ReadLine()
            If message IsNot Nothing Then
                ' Print the message to the console
                Console.WriteLine(message)

                ' Split the message into words
                Dim words As String() = message.Split("!")

                ' Get the user who sent the message
                Dim user As String = words(0).Split(" ")(0).Substring(1)

                ' Check for the word "death"
                If words.Contains("death") Then
                    ' Check if the user has permission to use the "death" command
                    If CheckPermission(user) Then
                        ' Increment the death count
                        deathCount += 1

                        ' Respond with the current death count
                        writer.WriteLine("PRIVMSG " & channel & " :Kuolemia: " & deathCount)
                        writer.Flush()
                    Else
                        ' Respond with an error message
                        ' writer.WriteLine("PRIVMSG " & channel & " :Oops! You don't have permissions to use this command, @" & user.ToString)
                        writer.Flush()
                    End If
                ElseIf words.Contains("test") Then
                    ' Check if the user has permission to use the "help" command
                    If CheckPermission(user) Then
                        ' Add user
                        writer.WriteLine("PRIVMSG " & channel & " :Testing testing. Check check...")
                        writer.Flush()
                    Else
                        ' Respond with an error message
                        'writer.WriteLine("PRIVMSG " & channel & " :Oops! You don't have permissions to use this command, @" & user.ToString)
                        writer.Flush()
                    End If
                ElseIf message.StartsWith("PING") Then
                    ' Send a PONG response back to the server
                    writer.WriteLine("PONG " & message.Substring(5))
                    writer.Flush()
                ElseIf message.Contains("pizza") Then
                    ' Check if the user has permission to use the "pizza" command
                    If CheckPermission(user) Then
                        ' Read the list of fillings from the "pizza.txt" file
                        Dim fillings As String() = File.ReadAllLines("pizza.txt")
                        ' Create a new instance of the Random class
                        Dim rnd As New Random()
                        ' Generate pizza
                        ' Get 4 random fillings from the list
                        Dim selectedFillings As New List(Of String)()
                        For i As Integer = 0 To 3
                            Dim index As Integer = rnd.Next(0, fillings.Length)
                            selectedFillings.Add(fillings(index))
                        Next
                        ' Join the selected fillings into a single string
                        Dim pizza As String = String.Join(", ", selectedFillings)

                        ' Send a message to the channel with the selected fillings
                        writer.WriteLine("PRIVMSG " & channel & " :Tässä sinulle neljän (4) täytteen pizza: " & pizza & " PizzaTime")
                        writer.Flush()
                    Else
                        ' Respond with an error message
                        'writer.WriteLine("PRIVMSG " & channel & " :Oops! You don't have permissions to use this command, @" & user)
                        writer.Flush()
                    End If
                End If
            End If

        Loop Until message Is Nothing

    End Sub

    ''' <summary>
    ''' Reads the value of a specified key from the config file.
    ''' </summary>
    ''' <param name="key">The key to look up in the config file.</param>
    ''' <returns>The value associated with the key in the config file.</returns>
    Private Function ReadConfigValue(key As String) As String
        ' Read the config file into a dictionary
        Dim config = File.ReadAllLines("config.txt") _
            .Select(Function(x) x.Split("=")) _
            .ToDictionary(Function(x) x(0), Function(x) x(1))

        ' Return the value for the specified key
        Return config(key)
    End Function

    ''' <summary>
    ''' Checks if the user has permission to use a command.
    ''' </summary>
    ''' <param name="user">The user to check permissions for.</param>
    ''' <returns>True if the user has permission, False otherwise.</returns>
    Private Function CheckPermission(user As String) As Boolean
        ' Read the permissions file into a list
        Dim permissions = File.ReadAllLines("permissions.txt")

        ' Check if the user is in the list of allowed users
        Return permissions.Contains(user)
    End Function
End Module
