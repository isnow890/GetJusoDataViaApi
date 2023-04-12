using Microsoft.VisualBasic.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;




// See https://aka.ms/new-console-template for more information

class Program
{

    private const string API_KEY = "devU01TX0FVVEgyMDIzMDQxMjEwMDgzOTExMzY3ODA=";
    private const string API_URL = "https://business.juso.go.kr/addrlink/addrLinkApi.do";
    private static string? keyword;
    private int currentPage = 1;
    private int countPerPage;

    //private AddressResults addressResults = default!;


    private static string SearchURL =>
        $"{API_URL}" +
        $"?confmKey={API_KEY}" +
        $"&currentPage=0" +
        $"&countPerPage=1000" +
        $"&keyword={keyword}" +
        $"&resultType=json"+
        $"&hstryYn=Y"
        ;



    public async static Task Main(string[] args)
    {


        while (true)
        {
            var resultList = new DTO();
            Console.WriteLine("아래에 검색하고자 하는 주소를 입력해주세요.");
            keyword = Console.ReadLine();

            var json = await GetAsync(SearchURL);
            var jsonDoc = JsonDocument.Parse(json);
            var root = jsonDoc.RootElement;
            var result = root.GetProperty("results");

            resultList = JsonSerializer.Deserialize<DTO>(result.ToString());



            var seq = 0;

            foreach (var cc in resultList.Juso)
            {
                Console.WriteLine($"{++seq}. ");
                Console.WriteLine($"우편번호 : {cc.우편번호}");
                Console.WriteLine($"도로명주소 : {cc.도로명주소}");
                Console.WriteLine($"건물명 : {cc.건물명}");
                Console.WriteLine($"영문도로명주소 : {cc.영문도로명주소}");
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);

            Console.WriteLine($"======================================");
            Console.WriteLine($"결과");
            Console.WriteLine($"총 {resultList.Common.토탈카운트}건");
            Console.WriteLine($"에러메시지 : {resultList.Common.에러메시지}");
            Console.WriteLine($"에러코드 : {resultList.Common.에러코드}");
            Console.WriteLine($"======================================");



        }



    }

    public static async Task<string> GetAsync(string url)
    {
        using HttpClient? httpClient = new HttpClient();
        HttpResponseMessage? res = await httpClient.GetAsync(url);
        return await res.Content.ReadAsStringAsync();
    }
    

    public class DTO
    {
        [JsonPropertyName("common")]
        public Common Common { get; set; }

        [JsonPropertyName("juso")]
        public List<Juso> Juso{ get; set; }
    }


    public class Common
    {
        [JsonPropertyName("totalCount")]
        public String 토탈카운트 { get; set; }
        [JsonPropertyName("currentPage")]
        public String 현재페이지 { get; set; }
        [JsonPropertyName("countPerPage")]
        public String 페이지당카운트 { get; set; }
        [JsonPropertyName("errorCode")]
        public String 에러코드 { get; set; }
        [JsonPropertyName("errorMessage")]
        public String 에러메시지 { get; set; }
    }


    public class Juso
    {
        [JsonPropertyName("roadAddr")]
        public string 도로명주소 { get; set; }
        [JsonPropertyName("roadAddrPart1")]
        public string 도로명주소1 { get; set; } 
        [JsonPropertyName("roadAddrPart2")]
        public string 도로명주소2 { get; set; } 
        [JsonPropertyName("ibunAddr")]
        public string 지번 { get; set; } 
        [JsonPropertyName("engAddr")]
        public string 영문도로명주소 { get; set; } 
        [JsonPropertyName("zipNo")]
        public string 우편번호 { get; set; }
        [JsonPropertyName("admCd")]
        public string 행정구역코드 { get; set; }
        [JsonPropertyName("rnMgtSn")]
        public string 도로명코드 { get; set; }
        [JsonPropertyName("bdMgtSn")]
        public string 건물관리번호 { get; set; }
        [JsonPropertyName("detBdNmList")]
        public string 상세건물명 { get; set; }
        [JsonPropertyName("bdNm")]
        public string 건물명 { get; set; }
        [JsonPropertyName("bdKdcd")]
        public string 공동주택여부 { get; set; }
        [JsonPropertyName("siNm")]
        public string 시도명 { get; set; } 
        [JsonPropertyName("sggNm")]
        public string 시군구명 { get; set; }
        [JsonPropertyName("emdNm")]
        public string 읍면동명 { get; set; }
        [JsonPropertyName("liNm")]
        public string 법정리명 { get; set; }
        [JsonPropertyName("Rn")]
        public string 도로명 { get; set; }
        [JsonPropertyName("udrtYn")]
        public string 지하여부 { get; set; }
        [JsonPropertyName("buldMnnm")]
        public string 건물본번 { get; set; }
        [JsonPropertyName("buldSlno")]
        public string 건물부번 { get; set; }

    }



}



