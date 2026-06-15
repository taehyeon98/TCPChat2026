using System.Collections.Concurrent;
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
    
    //연결된 클라이언트 저장할 스레드 환경에서 사용할 Dic(Thread-safe dictionary)
    private readonly ConcurrentDictionary<string, ConnectedClient> _connectedClients;
    
    //생성자(Constructor)
    public ChatServer(int port)
    {
        _port = port;
        _isRunning = false;
        _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();
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
        
        //백그라운드 스레드로 클라이언트 연결 수락시작.
        _ = Task.Run(AcceptClientAsync);
        
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
                
                //접속한 클라이언트 저장
                var infoConnectedClient = new ConnectedClient(client);
                //클라이언트 목록에 추가
                //_connectedClients.TryAdd(키,값);
                //Indexer방식
                _connectedClients[infoConnectedClient.ClientId] = infoConnectedClient;

                //클라이언트로 부터 메시지 수신 시작(비동기)
                _ = Task.Run(infoConnectedClient.ReceiveMessageAsync);
                
                //접속한 클라이언트 수 출력
                Console.WriteLine($"[정보] 현재 접속한 클라이언트 수 : {_connectedClients.Count}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}