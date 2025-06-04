param([string]$PlatformName)

# 플랫폼 이름 매핑
switch ($PlatformName) {
    "Windows_vc160" { $PlatformName = "Windows" }
    "HTML5" { $PlatformName = "Web" }
    default { $PlatformName = $PlatformName }
}

# 원본 경로 (사운드뱅크 생성 위치)
$sourcePath = "C:\GitHub\8thExit_CopyProject\WwiseProject\GeneratedSoundBanks\$PlatformName"
# 타겟 경로 (Unity StreamingAssets 경로)
$targetPath = "C:\GitHub\8thExit_CopyProject\Assets\StreamingAssets\Audio\GeneratedSoundBanks\$PlatformName"

Write-Host "==========================================="
Write-Host "📦 PLATFORM          : $PlatformName"
Write-Host "📂 SOURCE PATH       : $sourcePath"
Write-Host "📂 TARGET PATH       : $targetPath"
Write-Host "==========================================="

# 경로 유효성 체크
if (-Not (Test-Path $sourcePath)) {
    Write-Host "❌ SOURCE PATH DOES NOT EXIST!"
    exit 1
}

# .bnk/.json 파일 존재 여부 확인
$bnkFiles = Get-ChildItem "$sourcePath" -Filter *.bnk -ErrorAction SilentlyContinue
$jsonFiles = Get-ChildItem "$sourcePath" -Filter *.json -ErrorAction SilentlyContinue

if ($bnkFiles.Count -eq 0 -and $jsonFiles.Count -eq 0) {
    Write-Host "❌ NO .bnk or .json FILES FOUND IN SOURCE!"
    exit 1
} else {
    Write-Host "✅ FOUND FILES:"
    foreach ($file in $bnkFiles + $jsonFiles) {
        Write-Host "   - $($file.Name)"
    }
}

# 타겟 폴더 없으면 생성
if (-Not (Test-Path $targetPath)) {
    try {
        New-Item -ItemType Directory -Path $targetPath -Force | Out-Null
        Write-Host "📁 Created target directory"
    } catch {
        Write-Host "⚠️ Failed to create directory: $targetPath"
        exit 1
    }
}

# 복사 실행
Write-Host "🚚 COPYING FILES..."
try {
    Copy-Item "$sourcePath\*.bnk" $targetPath -Force -ErrorAction Stop
    Copy-Item "$sourcePath\*.json" $targetPath -Force -ErrorAction Stop
    Write-Host "✅ COPY SUCCESSFUL"
} catch {
    Write-Host "❌ COPY FAILED: $($_.Exception.Message)"
    exit 1
}

Write-Host "🎉 SCRIPT COMPLETED SUCCESSFULLY"