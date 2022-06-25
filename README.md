# My_new_chat_app_project
Author: Hoang Minh Le (511907)  
Description: The project is about creating a chat application which contains UI, and chat server for it.<br>
# Table of content
1. [Introduction](#introduction)
2. [User guildline](#User)
3. [Design](#Design)  
   3.1 [Algorithm](#algo)  
   3.2 [Packet](#packet)  
   3.3 [Chat application](#app)  
   3.4 [Chat server](#server)  
4. [Bugs still need to be fixed](#bugs)
5. [Reference](#reference)

## Introduction <a name=introduction/>
The chat app project is my product in two weeks. The chat app which I creates is able to send text message, file and allow users to change their avatar. I would love to implement voice call and video call into it. However, they are so complicated that I have to abandon them. 
## User guildline <a name=User/>
First, please visit my respository on [Github](https://github.com/yourcomrade/My_new_chat_app_project/tree/main)  
Then there are 2 options: either dowload all the file and compile them using Visual Studio or Jetbrain Rider. The project is coded entirely in C# and .Net Framework v4.7; or click into Chat_server folder and Bin/Debug and dowload the exe file. The same thing need to be done with Project_chat_app_and_server. Please run the server first befor running the chat app.   
Here is what chat app UI looks like
![Chat app](https://imgur.com/610dSsP.png)
Please note that the avatar can only be change after joining the server if not, the application will crash
Here is the result after joining the server and changing the avatar as well as send text message:
![2 chat app](https://imgur.com/KtZW7Mz.png)
If users want to send a file to other users, just click on the plus icon, the file can be chosen from File Dialog and the file is received in the folder which the chat_app is located:
![Imgur](https://imgur.com/2wumvYU.png)
If user wants to disconnect, just click on Disconnect button
## Design <a name=Design/>
### Algorithm <a name=algo/>
The design of the how the interaction between user, chat application and chat server can be described in this sequence diagram
![Sequence](https://imgur.com/PnHcrLo.png)
### Packet <a name=packet/>
The packet is the information sending over the network. Below is the image of packet for sending message
![The message packet](https://imgur.com/mGCWIXx.png)<br>
Here is the image of packet for sending a file:
![file packet](https://imgur.com/mWjUJUo.png)
The code is the signal for the both chat app and server to recognize what kind of message is expected to received after that so that they can prepare. 
Here is the code:  
1: Request connect to server from user and verify connection from server to server  
2: Sending text message from user to server   
3: Server broadcasts text message from 1 user to other users  
4: User send file to server  
5: Server broadcasts the file sent by 1 user to other users  
6: Server broadcasts to other users that 1 user has joined the chat  
7: User changes their avatar and server broadcast the url of the avatar to other uses  
10: User disconnects the server  
### Chat application <a name=app/>
The chat app is designed based on MVVM which is popular because it is a WPF application. In addition, the MVVM is the main design pattern of for application created from WPF. The Model contains the user data and message data, the View has only xaml files and the ViewModel contains MyMainViewModel class and UserInfoViewModel class to observe and bindind data. Other support classes are to help reading messages and files as well as send them.
Here is UML class for chat application. 
![Chat app uml class](https://imgur.com/TAdKfRm.png)
### Chat server <a name=server/>
The chat sever has only function as a broadcaster to all the users. It broadcasts the text message, the file message, the new user who has joint, user who has left and user who has changed their avatar. Here is the UML class of chat server
![chat server uml class](https://imgur.com/WHKk9gJ.png)
## Bugs still need to be fixe <a name=bugs/>
The problem with disconnection has not been fixed yet. Although I tried to search on Internet, it cannot provide me much result. The problem is when user click on the disconnect button, the program will crash due to some exceptions related to closing socket forcibly.
## Reference <a name=reference/>
Tutorial to buil chat app  
https://www.youtube.com/watch?v=I-Xmp-mulz4
https://www.youtube.com/watch?v=V9DkvcT27WI
https://www.youtube.com/playlist?list=PLrW43fNmjaQVYF4zgsD0oL9Iv6u23PI6M  
Command Binding  
 https://brunolm.wordpress.com/2015/03/01/icommand-and-relaycommand/  
  https://stackoverflow.com/questions/48527651/full-implementation-of-relay-command-can-it-be-applied-to-all-cases
  