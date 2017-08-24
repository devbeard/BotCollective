# BotCollective

A collection of bots and logic apps I have created to act as automated personalities in Slack, Github and so on. Gradually moving these from internal git repos to open source.

(Thanks to Håkon for pushing me on this.)

## BirthdayBot

An ASP.NET MVC app (created before the glorious days of .NET Core) that can check an Azure storage table for people to congratulate with their birthday Monday-Sunday, congratulate people on Friday if they have upcoming days in Sat or Sun, alert managers if any anniversaries (20,30,40,50 and so on) are upcoming and present a simple admin user interface.

Clearly not very robust or generalized for others uses (it even has the text in Norwegian), but a start. Pull requests very welcome.

## Other bots

These are some other bots I will try to move to this repository:

 * JokeBot - get a random Norwegian joke
 * YrBot - what is the forecast for this afternoon for my path home
 * BergenKinoBot - what movies are playing right now at Bergen Kino
 * GithubEnterpriseBot - alerts about new PRs for me and can give me stats for waiting PRs
 * BloodBot - the most emo s#*! throwing, miserable quote spewing automaton you will ever encounter... only install if you are way to chipper
