using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;

namespace TCPChatServer;

public class ChatServer
{
    //TCP프로토콜을 사용해서 클라이언트가 접속하기를 대기하는 클래스 : Listener Server
    
    //소켓(Socket)
    private TcpListener? _listener;
    //서버 실행 여부
    private bool _isRunning;
    //서버 포트
    private int _port;
    
    //생성자(Constructor)
    public ChatServer(int port)
    {
        _port = port;
        _isRunning = false;
    }
    
    //서버 시작
    public void StartServer()
    {
        if (_isRunning)
        {
            Console.WriteLine("서버 이미 실행중.");
            return;
        }
        
        //TCP 소켓 Open작업 = TCP 리스너 초기화
        _listener = new TcpListener(IPAddress.Any,  _port);
        _listener.Start();//소켓 메서드
        _isRunning = true;
        Console.WriteLine($"서버 정상 실행. 포트번호 : {_port} ");
    }
}