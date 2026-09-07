# Unity_2D_Portfolio
![Unity](https://img.shields.io/badge/Unity-6000-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-PC-blue)
<!--- 
## 0.목차
-[프로젝트 소개](#1.-프로젝트-소개)



-[핵심 기능](#핵심-기능)

-[기술 스택](#기술-스택)

-[문제 상황 및 해결 방법](#문제-상황-및-해결-방법) --->

## 1. 프로젝트 소개
<div align="center">
  <img src="https://github.com/user-attachments/assets/314f498e-c487-4c25-9192-2e2b2b770e3e" width="100%">
<br>
<게임 메인 이미지>
<br>
<br>


https://github.com/user-attachments/assets/374db197-5e83-4456-84c6-760bcdece79e




<지스타 트레일러>

<br>
<br>
<div align="left">
  
●  개발 기간: 2025.03~2025.11

●  개발 인원: 5인(기획 1, 개발 1, 아트 3)
<br>
●  Unity 2D 액션 플랫포머 게임입니다.
<br>
●  대학 진학 중 졸업 작품으로 진행한 프로젝트입니다.
<br>
●  본 Repository에는 저작권 문제로 인해 코드만 업로드 되어 있습니다.
<br>
<br>
<table align="center">
  <tr>
    <td><img src="https://github.com/user-attachments/assets/ff09c4af-fdaa-40e0-9aff-dcb7e6783cf4" width="400" height="250"></td>
    <td><img src="https://github.com/user-attachments/assets/1a42809e-84ea-4f0d-a781-7993784b5804" width="400" height="250"></td>
  </tr>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/c32b9c13-d258-4d49-8df6-7a1230c6d0e4" width="400" height="250"></td>
    <td><img src="https://github.com/user-attachments/assets/e2aa5bda-1a1a-4ad2-bd13-1ba7ead6d804" width="400" height="250"></td>
  </tr>
</table>
<div align="center">
<인게임 이미지>
<br>
<div align="left">
변신 시스템을 가미한 전통적인 2D 액션 플랫포머 게임입니다.
보스를 처치하며 세상에 색과 빛을 되찾아가는 모험을 떠납니다.
색을 되찾아 갈 때 마다 새로운 무기와 스킬을 얻을 수 있으며, 이를 통해 상황과 플레이어의 취향에 맞는 다양한 액션을 느낄 수 있습니다.

## 2. 개발 환경
<br>
● Unity 6000.0.44f1
<br>
● C#
<br>
● Visual Studio Code

## 3. 사용 기술
| 기술 | 설명 |
| --- | --- |
| 디자인 패턴 | Singleton 패턴을 이용한 코드 개선 <br> Enum 상태 정보 및 싱글톤 활용으로 플레이어 공격 스킬의 쿨타임 적립 및 최적화|
| URP Renderer | 포스트 프로세싱 구현 |
| Unity Shader | 포스트 프로세싱에 최적화된 메테리얼 자체 제작 |
| 물리 충돌 최적화 | Physics2D.BoxCast 및 OverlapBoxAll을 활용하여 접지 판정 및 스킬 범위 충돌 감지 구현 |
| CineMachine | 카메라 연출 구현 |
| UI 자동화 | Enum 상태정보와 연계된 UI 데이터 자동 동기화 |


## 4. 조작 방법

| 입력 | 동작 |
| --- | --- |
| A / D | 좌우 이동 |
| Space | 점프 |
| S + Space | 발판 아래로 통과 |
| Left Shift | 대시 |
| 마우스 좌클릭 | 기본 공격 |
| 마우스 우클릭 (홀드) | 스킬 |
| Q | 왼쪽 인접 상태로 변신 |
| E | 오른쪽 인접 상태로 변신 |

<br>


## 5. 트러블슈팅

**대시 스킬의 벽 충돌 판정 오류 (Raycast → BoxCast)**

블루 상태 스킬 사용 중 얇은 기둥·모서리를 그대로 통과하는 버그 발생. 원인은 벽/플랫폼 판정에 `Physics2D.Raycast` 사용 — 두께 없는 선(line)만 검사해, 빠른 이동 시 프레임 사이에서 장애물을 스쳐 지나가면 충돌 미검출됨.

```csharp
// 기존 — 얇은 Raycast 두 개로 판정
RaycastHit2D hit  = Physics2D.Raycast(rigid.position, direction, checkDistance, LayerMask.GetMask("Wall"));
RaycastHit2D hit2 = Physics2D.Raycast(rigid.position, direction, checkDistance, LayerMask.GetMask("Platform"));
if (hit.collider != null || hit2.collider != null) break;
```

캐릭터 콜라이더 크기를 반영한 `Physics2D.BoxCast`로 교체, 면적 기반 판정으로 전환함.

```csharp
// 수정 — 캐릭터 두께를 반영한 BoxCast로 교체
Vector2 origin = (Vector2)transform.position + capsuleCollider.offset;
Vector2 size = new Vector2(capsuleCollider.size.x, 0.8f);

RaycastHit2D hit = Physics2D.BoxCast(
    origin, size, 0f, direction, checkDistance,
    LayerMask.GetMask("Wall", "Platform", "Board")
);
if (hit.collider != null) break;
```

수정 후 통과 현상 재현되지 않음. 동일 원리로 접지 판정(`CheckGrounded`)에도 BoxCast 적용해 발판 모서리 착지 불안정 문제 함께 방지함.

<br>



## 6. 플레이 영상

https://youtu.be/TjwIxO0GERE

## 7. 다운로드 링크
https://drive.google.com/file/d/14C9ZRfjmuWxwsIqsy7G-Xf4BqqQzjwn9/view?usp=sharing


