/*
	Ditto is an application used for copying multiple items to the clipboard.
    This script pastes multiple items that have been copied from Ditto's clipboard.
*/

#SingleInstance, Force
#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.

; Import variables from other files
InputBoxWidth = 400
InputBoxHeight = 150
ImagesPastedSoFar = 0
FileRead, DelayGeneral, helper\DelayGeneral\current.txt
FileRead, HotkeyForPasteMultipleImagesWithClipboardSoftware, helper\HotkeyForPasteMultipleImagesWithClipboardSoftware\current.txt
Hotkey,%HotkeyForPasteMultipleImagesWithClipboardSoftware%,Button
Return
Button:

; Prompt user for how many images to be pasted
InputBox, NumberOfImagesCopied, pasteImagesAndAudioWithClipboardSoftware, How many images should be pasted from Ditto?, , InputBoxWidth, InputBoxHeight

; Delays (called by Sleep) are needed because neither the computer nor Ditto operate instantaneously
Sleep, DelayGeneral
; Paste all images from clipboard software into the text box
if (ClipboardSoftware == "ditto") {
	Loop, % NumberOfImagesCopied
	{
		Run, helper\ActivateDitto\current.ahk
		Sleep, DelayGeneral
		Loop, % NumberOfImagesCopied-1
		{
			SendEvent, {Down}
			Sleep, DelayGeneral
		}
		Send, {Enter}
		Sleep, DelayGeneral
	}
} else {
	Loop, % NumberOfImagesCopied
	{
		Run, helper\ActivateWindowsClipboard\current.ahk
		Sleep, DelayGeneral
		Loop, % NumberOfImagesCopied - ImagesPastedSoFar - 1
		{
			SendEvent, {Down}
			Sleep, DelayGeneral
		}
		Send, {Enter}
		Sleep, DelayGeneral
		ImagesPastedSoFar++
	}
}
; Go back to the very first image
Loop, % NumberOfImagesCopied
{
	SendEvent, {Left}
	Sleep, DelayGeneral
}
; Put each image on a separate line
Loop, % NumberOfImagesCopied-1
{
	SendEvent, {Right}
	Sleep, DelayGeneral
	SendEvent, {Enter}
	Sleep, DelayGeneral
}

return