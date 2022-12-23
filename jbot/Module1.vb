'---------------------
'jBot by Alt
'---------------------
'Easy to customize!
'---------------------
'config.txt for server and bot settings
'permissions.txt for usernames to have access to the bot!
'jbot/announcements.txt for automatically posted lines
'jbot/pizza.txt for toppings for !pizza command....
'---------------------
Imports System.Drawing
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Timers
Imports System.Globalization

Module Module1
    Sub Main()
        Console.Title = "jBot v." & My.Application.Info.Version.ToString
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("   _ _____     _   ")
        Console.WriteLine("  |_| __  |___| |_ ")
        Console.WriteLine("  | | __ -| . |  _|")
        Console.WriteLine(" _| |_____|___|_|  ")
        Console.WriteLine("|___|by Alt v." & My.Application.Info.Version.ToString)
        Console.WriteLine("----------------------------")
        Console.ResetColor()
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("Make sure you configure everything properly!" & Environment.NewLine & "* config.txt for bot settings" & Environment.NewLine & "* permissions.txt for user permissions" & Environment.NewLine & "* jbot/pizza.txt for pizza toppings" & Environment.NewLine & "* jbot/announcements.txt for automatically posted lines of text!" & Environment.NewLine & "* jbot/deaths.txt for death counter deaths")
        Console.ResetColor()
        Console.ForegroundColor = ConsoleColor.Cyan
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

        ' Read the announcements from the "announcements.txt" file
        Dim announcements As List(Of String) = ReadAnnouncements()
        ' Initialize the announcement index to 0
        Dim announcementIndex As Integer = 0
        ' Check the value of "show-announcements" in the config file
        Dim showAnnouncements As String = ReadConfigValue("showAnnouncements")

        'Read the announcement timer
        Dim announcementTimer As Integer = 1000 * 60 * ReadConfigValue("announcementTime")



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
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("Connected to " & server & " as: " & botName)
        Console.ResetColor()
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("----------------------------")
        Console.ResetColor()

        ' Set up a timer to send an announcement if "show-announcements" is set to 1 in the config file
        If showAnnouncements = "1" Then
            Dim timer As New Threading.Timer(Sub()
                                                 If announcementIndex >= announcements.Count Then
                                                     announcementIndex = 0
                                                 End If
                                                 Dim announcement As String = announcements(announcementIndex)
                                                 writer.WriteLine("PRIVMSG " & channel & " :" & announcement)
                                                 writer.Flush()
                                                 announcementIndex += 1
                                             End Sub, Nothing, 0, announcementTimer)
        End If

        ' Keep reading messages from the IRC server
        Try
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

                    ' ________                 __.__            
                    '\______ \   ____ _____ _/  |_|  |__   ______
                    ' |    |  \_/ __ \\__  \\   __\  |  \ /  ___/
                    ' |    `   \  ___/ / __ \|  | |   Y  \\___ \ 
                    '/_______  /\___  >____  /__| |___|  /____  >
                    '        \/     \/     \/          \/     \/ 
                    'Death counter -> Type !death -> Deaths will increase by +1. To reset deaths, write 0 to deaths.txt.

                    ' Check for the word "death"
                    If words.Contains("death") Then
                        ' Check if the user has permission to use the "death" command

                        ' Check if the deaths.txt file exists
                        If Not File.Exists("jbot/deaths.txt") Then
                            ' If the file does not exist, create it and write 0 to it
                            File.WriteAllText("jbot/deaths.txt", "0")
                        End If

                        If CheckPermission(user) Then
                            ' Read the death count from the file
                            deathCount = Convert.ToInt32(File.ReadAllText("jbot/deaths.txt"))
                            ' Increment the death count
                            deathCount += 1

                            ' Respond with the current death count
                            writer.WriteLine("PRIVMSG " & channel & " :Kuolemia: " & deathCount)
                            ' Write the death count to the file
                            File.WriteAllText("jbot/deaths.txt", deathCount.ToString())
                            writer.Flush()
                        Else
                            ' Respond with an error message
                            ' writer.WriteLine("PRIVMSG " & channel & " :Oops! You don't have permissions to use this command, @" & user.ToString)
                            writer.Flush()
                        End If

                        '   __________
                        '  /  _  \\_ |__   ____  __ ___/  |_ 
                        ' /  /_\  \| __ \ /  _ \|  |  \   __\
                        '/    |    \ \_\ (  <_> )  |  /|  |  
                        '\____|__  /___  /\____/|____/ |__|  
                        '        \/    \/                    
                    ElseIf words.Contains("jbot") Then
                        ' Check if the user has permission to use the this command
                        If CheckPermission(user) Then
                            ' Add user
                            writer.WriteLine("PRIVMSG " & channel & " :jBot v." & My.Application.Info.Version.ToString & " by Alt. Botti on kehityksessä ja komennot vaativat käyttöoikeuden. Käyttöoikeus voidaan myöntää luotettaville henkilöille. Striimaaja määrittää itse, kuka on luotettava ja kuka ei.")
                            writer.Flush()
                        End If

                        '__________
                        '\______   \____   ____    ____  
                        ' |     ___/  _ \ /    \  / ___\ 
                        ' |    |  (  <_> )   |  \/ /_/  >
                        '|____|   \____/|___|  /\___  / 
                        '                     \//_____/  
                        'This sends PONG back to Twitch server when PING is received.
                    ElseIf message.StartsWith("PING") Then
                        ' Send a PONG response back to the server
                        writer.WriteLine("PONG " & message.Substring(5))
                        writer.Flush()
                        '__________.__
                        '\______   \__|____________________   
                        ' |     ___/  \___   /\___   /\__  \  
                        ' |    |   |  |/    /  /    /  / __ \_
                        ' |____|   |__/_____ \/_____ \(____  /
                        '                  \/      \/     \/ 
                        'Pizza Generator is a popular feature in my bots so of course jBot has one too!
                        'Add toppings to pizza.txt and let bot generate 4 topping pizza for you.
                    ElseIf message.Contains("pizza") Then
                        ' Check if the user has permission to use the "pizza" command
                        If CheckPermission(user) Then
                            ' Read the list of toppings from the "pizza.txt" file
                            Dim toppings As String() = File.ReadAllLines("jbot/pizza.txt")
                            ' Create a new instance of the Random class
                            Dim rnd As New Random()
                            ' Generate pizza
                            ' Get 4 random toppings from the list
                            Dim selectedtoppings As New List(Of String)()
                            For i As Integer = 0 To 3
                                Dim index As Integer = rnd.Next(0, toppings.Length)
                                selectedtoppings.Add(toppings(index))
                            Next
                            ' Join the selected toppings into a single string
                            Dim pizza As String = String.Join(", ", selectedtoppings)

                            ' Send a message to the channel with the selected toppings
                            writer.WriteLine("PRIVMSG " & channel & " :Tässä sinulle neljän (4) täytteen pizza: " & pizza & " PizzaTime")
                            writer.Flush()
                        Else
                            ' Respond with an error message
                            'writer.WriteLine("PRIVMSG " & channel & " :Oops! You don't have permissions to use this command, @" & user)
                            writer.Flush()

                        End If
                        '___________.__
                        '\__    ___/|__| _____   ____  
                        ' |    |   |  |/     \_/ __ \ 
                        ' |    |   |  |  Y Y  \  ___/ 
                        ' |____|   |__|__|_|  /\___  >
                        '                   \/     \/ 
                        'Displays the current time
                    ElseIf message.Contains("time") Then
                        ' Check if the user has permission to use the "time" command
                        If CheckPermission(user) Then
                            Dim currentTime As String = DateTime.Now.ToString("HH:mm:ss tt")
                            ' Send the current time as a message
                            writer.WriteLine("PRIVMSG " & channel & " :" & currentTime)
                            writer.Flush()
                        Else
                            ' Respond with an error message
                            'writer.WriteLine("PRIVMSG " & channel & " :Oops! You don't have permissions to use this command, @" & user)
                            writer.Flush()
                        End If
                        '_________.          .__.__
                        '/  __  \_ |__ _____  |  | |  |  
                        ' >      <| __ \\__  \ |  | |  |  
                        '/   --   \ \_\ \/ __ \|  |_|  |__
                        '\______  /___  (____  /____/____/
                        '       \/    \/     \/           
                        'Ask the mighty 8 ball
                    ElseIf message.Contains("8ball") Then
                        ' Check if the user has permission to use the "8ball" command
                        If CheckPermission(user) Then
                            ' Read the list of answers from the "8ball.txt" file
                            Dim answers As String() = File.ReadAllLines("jbot/8ball.txt")
                            ' Create a new instance of the Random class
                            Dim rnd As New Random()
                            ' Generate a random answer
                            Dim index As Integer = rnd.Next(0, answers.Length)
                            Dim answer As String = answers(index)

                            ' Send the answer to the channel
                            writer.WriteLine("PRIVMSG " & channel & " :" & answer)
                            writer.Flush()
                        Else
                            ' Respond with an error message
                            'writer.WriteLine("PRIVMSG " & channel & " :Oops! You don't have permissions to use this command, @" & user)
                            writer.Flush()
                        End If
                    End If
                End If

            Loop Until message Is Nothing
        Catch ex As Exception
            Console.WriteLine("You encounted a known bug. Restart the bot to fix this issue..")
        End Try
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
    ' Read the announcements from the "announcements.txt" file
    Function ReadAnnouncements() As List(Of String)
        Dim announcements As New List(Of String)
        Using reader As New StreamReader("jbot/announcements.txt")
            Dim line As String
            Do
                line = reader.ReadLine()
                If line IsNot Nothing Then
                    announcements.Add(line)
                End If
            Loop While line IsNot Nothing
        End Using
        Return announcements
    End Function
End Module
