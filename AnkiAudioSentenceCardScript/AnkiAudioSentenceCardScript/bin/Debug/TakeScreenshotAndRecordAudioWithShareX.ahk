/*
	This script outputs a screenshot and an audio recording of any video content to two files, by calling helper in ShareX.

*/

#SingleInstance, Force
#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.

; Import variables from other files
FileRead, IsRecordingAudio, helper\IsRecordingAudio\current.txt
FileRead, DelayGeneral, helper\DelayGeneral\current.txt
FileRead, DelayForRecordingToStart, helper\DelayForRecordingToStart\current.txt
FileRead, WhenToTakeScreenshotWithShareX, helper\WhenToTakeScreenshotWithShareX\current.txt

FileRead, HotkeyForTakeScreenshotAndRecordAudioWithShareX, helper\HotkeyForTakeScreenshotAndRecordAudioWithShareX\current.txt
Hotkey,%HotkeyForTakeScreenshotAndRecordAudioWithShareX%,Button
Return
Button:

; If the audio is not recording
if (IsRecordingAudio = 0) {
	; 0 = takes screenshot at beginning, 1 = takes screenshot at end. 2 (or any number that's not 0 or 1) = doesn't take any screenshots
	if (WhenToTakeScreenshotWithShareX = 0) {
		Run, helper\TakeScreenshotWithShareX\current.ahk
		Sleep, DelayGeneral*2
	}
	
	Run, helper\ToggleRecordAudioWithShareX\current.ahk
	Sleep, DelayForRecordingToStart
	Run, helper\PlayPauseVideo\current.ahk

	IsRecordingAudio = 1
	
	; Otherwise if the audio is recording
} else {
	Run, helper\PlayPauseVideo\current.ahk
	Sleep, DelayGeneral
	Run, helper\ToggleRecordAudioWithShareX\current.ahk

	if (WhenToTakeScreenshotWithShareX = 1) {
		Sleep, DelayGeneral*2
		Run, helper\TakeScreenshotWithShareX\current.ahk
	}

	IsRecordingAudio = 0
}

; Update the new state of IsRecordingAudio
FileDelete, helper\IsRecordingAudio\current.txt
FileAppend, %IsRecordingAudio%, helper\IsRecordingAudio\current.txt

return