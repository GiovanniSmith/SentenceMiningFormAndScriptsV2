/*
	Ditto is an application used for copying multiple items to the clipboard.
    This script pastes two items that have been copied from Ditto's clipboard (a screenshot and an audio recording).
*/

#SingleInstance, Force
#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.

; Import variables from other files
FileRead, DelayGeneral, helper\DelayGeneral\current.txt
FileRead, WhenToTakeScreenshotWithShareX, helper\WhenToTakeScreenshotWithShareX\current.txt
FileRead, HotkeyForPasteImageAndAudioWithClipboardSoftware, helper\HotkeyForPasteImageAndAudioWithClipboardSoftware\current.txt
FileRead, ClipboardSoftware, helper\ClipboardSoftware\current.txt
Hotkey,%HotkeyForPasteImageAndAudioWithClipboardSoftware%,Button
Return
Button:

Sleep, DelayGeneral

if (ClipboardSoftware == "ditto") {
    if (WhenToTakeScreenshotWithShareX = 0) {
        ; if screenshot is taken at beginning (meaning it is copied 1st, and recording is copied 2nd)
        
        Loop, 2 {
            if (A_Index = 2) {
                Send, {Right}
                Sleep, DelayGeneral
                Send, {Enter}
                Sleep, DelayGeneral
            }
            Run, helper\ActivateDitto\current.ahk
            Sleep, DelayGeneral
            SendEvent, {Down}
            Sleep, DelayGeneral
            Send, {Enter}
            Sleep, DelayGeneral
        }
    } else if (WhenToTakeScreenshotWithShareX = 1) {
        ; if screenshot is taken at end (meaning it is copied 2nd, and recording is copied 1st)

        Loop, 2 {
            Run, helper\ActivateDitto\current.ahk
            Sleep, DelayGeneral
            if (A_Index = 1) {
                Send, {Enter}
                Sleep, DelayGeneral
            } else if (A_Index = 2) {
                Send, {Down}
                Sleep, DelayGeneral
            }
            Send, {Enter}
        }
    } else if (WhenToTakeScreenshotWithShareX = 2) {
        ; if not screenshot is taken (meaning only the recording was copied)

        Run, helper\ActivateDitto\current.ahk
        Sleep, DelayGeneral
        Send, {Enter}
    }
} else {
    if (WhenToTakeScreenshotWithShareX = 0) {
        ; if screenshot is taken at beginning (meaning it is copied 1st, and recording is copied 2nd)
        Run, helper\CtrlVToPaste.ahk
        Sleep, DelayGeneral
        Run, helper\HomeOnNumpad.ahk
        Sleep, DelayGeneral
        Send, {Enter}
        Sleep, DelayGeneral
        Send, {Up}
        Sleep, DelayGeneral
        Run, helper\ActivateWindowsClipboard\current.ahk
        Sleep, DelayGeneral
        Send, {Enter}
    } else if (WhenToTakeScreenshotWithShareX = 2) {
        ; if not screenshot is taken (meaning only the recording was copied)
        Run, helper\CtrlVToPaste.ahk
    }
}

return