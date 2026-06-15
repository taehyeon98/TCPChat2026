using System.Formats.Tar;

namespace TCPChatServer;

using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //Windows 콘솔에서 UTF-8 인코딩 설정
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        
        //기본 포트 번호 설정 0~65536
        //1000번 이하 포트는 미리 예약되어 있다.
        const int port = 7777;
        
        //채팅 서버 인스턴스 생성 및 실행.
        var server = new ChatServer(port);
        //서버 시작처리
        server.StartServer();
        
        Console.WriteLine("서버 종료시 아무키 입력");
        Console.ReadKey(true);//입력한 키 값 Hook, 화면에 출력되지 않게 하는 옵션
        
        //TODO:서버 정지 로직구현
        Console.WriteLine("서버 종료.");
    }
}