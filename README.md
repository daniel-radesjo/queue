# queue
Run commands from a given queue (queue.txt) from one or multiple computers.

Example (without created queue.txt):
```
q.exe
queue.txt doesn't exists, creating...
Rows in queue.txt: 0
Computer name: PC
```

Example (with commands in queue.txt):
```
type queue.txt
echo 1
echo 2
echo 3

q.exe
Rows in queue.txt: 3
Computer name: PC
Starting echo 1
Finished echo 1
Starting echo 2
Finished echo 2
Starting echo 3
Finished echo 3

type PC.finished.txt
echo 1
echo 2
echo 3

type PC.output.txt
1
2
3
```
