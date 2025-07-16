# Ping Check Server Down with LINE Notification

**PingCheckServerDown** is a .NET Framework application designed to automatically monitor the status of servers or network devices and send notifications to your LINE group when their status changes (e.g., from Online to Offline).

## ✨ Features

  * **Automatic Status Checks:** Runs in the background to periodically check server status at a configurable interval.
  * **LINE Notifications:** Instantly sends a message to a LINE group when a server's status changes.
  * **Offline Summary:** If a server goes offline, the notification will include a list of all problematic servers.
  * **Easy to Deploy:** Can be set up to run automatically using Windows Task Scheduler.
  * **Error Logging:** Creates a log file to record any errors that may occur during execution.

## ⚙️ How It Works

The application runs on a timer (`timer1`), with a default interval of 2 minutes.

1.  **Read Result File:** The program reads the entire content of the `.txt` file specified in the `App.config`.
2.  **Check Status:** It searches for lines containing the word "Offline" to assess the overall status.
3.  **Compare Status:** The application stores the most recent status ("Online" or "Offline") and compares it to the status from the current check.
4.  **Send Notification:** If the status changes (e.g., from "Online" to "Offline" or vice versa), the program calls the `LineNotify` function to send a message via the LINE API.
      * **When Offline:** It sends a notification message with a list of all servers detected as "Offline."
      * **When Back Online:** It sends a message indicating that the system is back to normal.

## 🛠️ Configuration

Before running the application, you must configure the `App.config` file, located in the `PingCheckServerDown/PingCheckServerDown` folder.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <appSettings>
        <add key="Token" value="paste token here"/>

        <add key="ReadFilePath" value="C:\PingCheck\Result.txt"/>

        <add key="LogFile" value="C:\PingCheck\LogError.txt"/>
    </appSettings>
</configuration>
```

**Description:**

  * `Token`: The token obtained from [LINE Notify](https://notify-bot.line.me/my/). This allows the program to send messages to your LINE group.
  * `ReadFilePath`: The full path to the `.txt` file containing the server ping results. The program will read this file to check the status.
  * `LogFile`: The full path to the `.txt` file that will be used to log any errors encountered by the program.

## 🚀 Installation and Usage

1.  **Prepare the `Result.txt` File:**

      * Create a file named `Result.txt` (or another name as configured in `ReadFilePath`).
      * This file should be regularly updated with the status of your servers (e.g., by a ping script).
      * **Example content:**
        ```
        ServerA - 192.168.1.1 - Online
        ServerB - 192.168.1.2 - Offline
        ServerC - 192.168.1.3 - Online
        ```

2.  **Configure `App.config`:**

      * Open the `App.config` file and update the `Token`, `ReadFilePath`, and `LogFile` values as described above.

3.  **Run the Program via Task Scheduler:**

      * Open **Task Scheduler** on Windows.
      * Select **Create Basic Task...**
      * Give the task a name and description.
      * **Trigger:** Set how often you want the program to run (e.g., Daily, Hourly, or When the computer starts).
      * **Action:** Select **Start a program**.
      * **Program/script:** Click `Browse...` and select the `PingCheckServerDown.exe` file.
      * Click **Finish** to create the task.

Once configured, the program will run automatically at the scheduled time and send notifications to your LINE group whenever a server's status changes.
