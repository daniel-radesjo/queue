#!/usr/bin/python
#Application for running commands from one or multiple computers from shared queue
import sys
import os.path
import socket
import time
from subprocess import Popen

version = "P1.0"

strQueueFile = "queue.txt"
strActiveFile = "%computername%.active.txt"
strFinishedFile = "%computername%.finished.txt"
strOutputFile = "%computername%.output.txt"

strCmd = ""
strComputerName = socket.gethostname() #Get computer name

def debug(strMessage):
    print(str(strMessage).strip())

if __name__ == '__main__':
    if not os.path.exists(strQueueFile):
        debug(strQueueFile + " doesn't exists, creating...")
        with open(strQueueFile, "w") as f:
            f.close()

    strActiveFile = strActiveFile.replace("%computername%", strComputerName)
    strFinishedFile = strFinishedFile.replace("%computername%", strComputerName)
    strOutputFile = strOutputFile.replace("%computername%", strComputerName)

    debug("Rows in " + strQueueFile + ": " + str(len(open(strQueueFile, "r").readlines())))
    debug("Computer name: " + strComputerName)

    #While commands exists in queue
    while len(open(strQueueFile, "r").readlines()) > 0:

        #Get top command from queue and replace queue with the rest
        with open(strQueueFile, "r+") as queue:
            lines = queue.readlines()
            strCmd = str(lines[0]).strip()
            queue.seek(0)
            queue.writelines(lines[1:])
            queue.truncate()
            queue.close()

        if len(str(strCmd)):
            debug("Starting " + strCmd)

            #Create active file
            with open(strActiveFile, "w") as active:
                active.write(strCmd)
                active.close()

            #Run command and pipe result to output file
            p = Popen(["cmd", "/c " + strCmd + " >> " + strOutputFile])
            p.wait()

            debug("Finished " + strCmd)

            #Update finished file
            with open(strFinishedFile, "a") as finished:
                finished.writelines(strCmd + "\r\n")
                finished.close()

            os.remove(strActiveFile)

        time.sleep(1)
