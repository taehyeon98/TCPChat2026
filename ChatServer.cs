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
    
    //서버 종료
    public void StopServer()
    {
        if (!_isRunning)
        {
            return;
        }
        
        _isRunning = false;
        _listener?.Stop();
        _listener = null;
        
        Console.WriteLine("서버 종료.");
    }
    
    //클라이언트 연결(접속 요청,비동기방식 처리)
    private async Task AcceptClientAsync()
    {
        Console.WriteLine("클라이언트 접속 대기중");
        //무한루프 돌면서 접속요청 대기처리
        while (_isRunning)
        {
            try
            {
                //클라이언트 연결 요청까지 대기
                var client = await _listener!.AcceptTcpClientAsync();
                
                //연결된 클라이언트 정보 출력
                var endPoint = client.Client.RemoteEndPoint;
                Console.WriteLine($"[연결] 클라이언트가 접속했습니다. : {endPoint}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}