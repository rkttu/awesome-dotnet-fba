#!/usr/bin/env dotnet
#:package Microsoft.SemanticKernel.Connectors.Ollama@1.61.0-alpha
#:package Microsoft.SemanticKernel.Agents.Core@1.61.0

#:property PublishAot=false

#pragma warning disable SKEXP0070

using OllamaSharp;
using Microsoft.SemanticKernel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using System.ComponentModel;

var showIndicator = true;

#if LINQPAD
#nullable enable
showIndicator = false;
#endif

var ollamaUri = new Uri("http://localhost:11434/");
var modelName = "coolsoon/kanana-1.5-8b";

var kernelBuilder = Kernel
	.CreateBuilder()
	.AddOllamaChatCompletion(modelName, ollamaUri);
var kernel = kernelBuilder.Build();

var agent = new ChatCompletionAgent()
{
	Kernel = kernel,
	Name = "지역 시간대 정보 에이전트",
	Instructions =
		"""
	    다양한 지역에 대한 질문에 답하세요.
	    프랑스의 경우 시간 형식(HH:MM)을 사용하세요.
	    HH는 00부터 23시까지, MM은 00부터 59분까지입니다.
	    """,
	Arguments = new KernelArguments(new PromptExecutionSettings
	{
		FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
	}),
};

var plugin = KernelPluginFactory.CreateFromType<TimePlugin>(
	nameof(TimePlugin));

agent.Kernel.Plugins.Add(plugin);

// 상수 정의
const string SystemPrompt = """
You are a helpful AI assistant developed by Kakao with access to specific functions. 한국어와 영어 모두에서 정확한 함수 호출을 수행할 수 있습니다.

함수 호출 규칙:
1. **명시적으로 제공된 함수만 사용** - 함수를 임의로 생성하거나 환상하지 마세요
2. **필요시 함수 호출** - 사용자 요청이 외부 데이터나 함수가 제공할 수 있는 작업을 필요로 할 때 반드시 사용하세요
3. **미래 함수 호출 약속 금지** - 함수 호출이 필요하면 지금 실행하고, 그렇지 않으면 일반적으로 응답하세요
4. **필수 매개변수 누락시 질문** - 필수 매개변수가 누락된 경우 함수를 호출하기 전에 사용자에게 제공을 요청하세요
5. **선택적 매개변수는 요청하지 않음** - 필수 매개변수에 대해서만 명확화를 요청하세요

응답 형식:
- 함수 호출: 지정된 매개변수 이름과 타입에 정확히 맞는 유효한 JSON 반환
- 일반 응답: 도움이 되고 자연스러운 언어 응답 제공  
- 같은 응답에서 함수 호출과 일반 텍스트를 혼합하지 마세요

매개변수 처리:
- **필수 매개변수**: 명시적으로 제공되거나 컨텍스트에서 명확하게 추론 가능해야 함
- **선택적 매개변수**: 사용자가 명시적으로 언급한 경우에만 포함
- **데이터 타입**: 지정된 타입(string, number, boolean, array, object)을 엄격히 준수
- **Enum**: 제공된 enum 목록의 값만 사용

오류 방지:
- 호출 전 모든 필수 매개변수가 있는지 검증
- 정의된 대로 정확한 함수명 사용
- 정확한 JSON 스키마 구조 준수
- 스키마에 정의되지 않은 추가 필드 추가 금지

사용 가능한 함수들이 tools 매개변수로 제공됩니다. 정확성과 스키마 준수는 성공적인 함수 실행을 위해 매우 중요합니다.
""";

var exitCommands = new[] { "/exit", "/bye", "/quit", };
var clearCommands = new[] { "/clear", "/reset", "/restart", };
var helpCommands = new[] { "/help", "/h", "?" };

var helpText = $$"""
{{agent.Name}}

=== 명령어 ===
{{string.Join(", ", clearCommands)}} - 초기화
{{string.Join(", ", helpCommands)}} - 도움말
{{string.Join(", ", exitCommands)}} - 종료

=== 예시 ===
프랑스 일자흐의 시간은 몇 시입니까?
대한민국 서울과 뉴욕 사이의 시간차는 몇 시간인가요?
""";

var chat = new ChatHistory([
	new ChatMessageContent(AuthorRole.System, SystemPrompt)
]);

Console.WriteLine(helpText);

while (true)
{
	if (showIndicator)
	    Console.Write("You: ");

	string? input = Console.ReadLine();

	if (string.IsNullOrWhiteSpace(input)) continue;

	var cmd = input.ToLowerInvariant();

	// 명령어 처리
	if (exitCommands.Contains(cmd))
	{
		Console.WriteLine();
		Console.WriteLine("대화를 종료합니다. 안녕히 가세요!");
		break;
	}

	if (clearCommands.Contains(cmd))
	{
		chat.Clear();
		chat.Add(new ChatMessageContent(AuthorRole.System, SystemPrompt));

		Console.WriteLine();
		Console.WriteLine("대화 기록이 초기화되었습니다.");
		Console.WriteLine();
		continue;
	}

	if (helpCommands.Contains(cmd))
    {
		Console.WriteLine();
        Console.WriteLine(helpText);
        continue;
    }
    
    // AI와 대화
    chat.Add(new ChatMessageContent(AuthorRole.User, input));
	
	if (showIndicator)
	    Console.Write("AI: ");
    
    try
    {
        await foreach (var response in agent.InvokeAsync(chat))
        {
            if (!string.IsNullOrEmpty(response.Message?.Content))
                Console.Write(response.Message.Content);
            chat.Add(response);
        }

        Console.WriteLine();
		Console.WriteLine();
	}
	catch (Exception ex)
    {
        Console.WriteLine($"오류: {ex.ToString()}");
		Console.WriteLine();
	}
}

public sealed class TimePlugin
{
    [KernelFunction]
    [Description(
        """
		특정 도시의 현재 시간을 반환합니다.
		AI는 사용자가 입력한 도시명을 Windows 표준 시간대 ID로 변환해서 이 함수를 호출해야 합니다.
		
		주요 시간대 ID:
		- 서울/한국: Korea Standard Time
		- 도쿄/일본: Tokyo Standard Time  
		- 베이징/중국: China Standard Time
		- 뉴욕/미동부: Eastern Standard Time
		- 시카고/미중부: Central Standard Time
		- LA/미서부: Pacific Standard Time
		- 런던/영국: GMT Standard Time
		- 파리/독일: W. Europe Standard Time
		- 시드니/호주: AUS Eastern Standard Time
		"""
        )]
    string GetCurrentTime(
        [Description("Windows 표준 시간대 ID (예: 'Korea Standard Time')")] string timeZoneId,
        [Description("사용자가 요청한 도시명 (표시용)")] string city = "")
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            var locationName = !string.IsNullOrEmpty(city) ? city : timeZoneId;

            return $"{locationName} 지역의 현재 시간은 {currentTime:HH:mm}입니다.";
        }
        catch (TimeZoneNotFoundException)
        {
            return $"시간대 '{timeZoneId}'를 찾을 수 없습니다. 올바른 Windows 시간대 ID를 사용해주세요.";
        }
        catch (Exception ex)
        {
            return $"시간 조회 중 오류가 발생했습니다: {ex.Message}";
        }
    }
}
