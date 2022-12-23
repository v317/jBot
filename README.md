# jBot
Twitch chat bot made in VB.NET

# Config
<b>Create "config.txt" and add following lines:</b>
<br />
server=irc.twitch.tv<br />
port=6667<br />
channel=#yourchannel<br />
botName=YourBotUsername<br />
oauth=oauth:BotsTokenHere<br />
showAnnouncements=0<br />
announcementTime=10
<br />
<br />
Also create "permissions.txt" and add usernames that you wish to have access to the bot.<br />
One username/line<br />
<br />
To use pizza generator that comes with the bot, create "pizza.txt" and add fillings to this file.<br />
One filling/line, no limit on how many lines of fillings. Bot will pick 4 randomly.<br />
Bot also comes with 8ball etc. Most commands are in Finnish language by default for pre-compiled build.<br />
<br />
<br />
# Config
showAnnouncements -> if set to 1 instead of 0, bot will automatically post random lines from jbot/announcements.txt<br />
announcementTime=10 -> This is time in minutes when bot posts a random line (10 minutes default)
<br />
<br />
<b>Pre-compiled release comes with all the files, so you only have to configure it!</b>
