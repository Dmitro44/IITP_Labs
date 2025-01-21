.model small
.stack 100h
.data
    array dw 99, ?, 100 dup(0)
    maxArraySize db 99
    mult db 10
    min_old dw ?
    max_old dw ?
    a dw 0
    b dw 0
    buffer db 5, ?, 6 dup('$')
    newRangeMsg db 0dh, 0ah, "Enter lower and upper bound of new range (-127..127): ", 0dh, 0ah, "$"
    newElementMsg db 0dh, 0ah, "Enter new element (-127...127), 99 elements max (enter b if you want to stop): ", 0dh, 0ah, "$"
    foundNotNumberError db 0dh, 0ah, "Enter a number please!", 0dh, 0ah, "$"
    foundDollarError db 0dh, 0ah, "Dollar sigh was found, enter a number please!", 0dh, 0ah, "$"
    emptyInputError db 0dh, 0ah, "Empty input, enter a number please!", 0dh, 0ah, "$"
    upperLowerThanLowerMsg db 0dh, 0ah, "Upper bound is lower than or equal to lower bound, this is incorrect!", 0dh, 0ah, "$"
    arraySizeErrorMsg db 0dh, 0ah, "Array should have at least 2 elements", 0dh, 0ah, "$"
    outOfRangeMsg db 0dh, 0ah, "Error: Value must be between -127 and 127", 0dh, 0ah, "$"
    modifiedArrayMsg db 0dh, 0ah, "Modified array:", 0dh, 0ah, "$"
    minusSign db "-$"
    newLine db 0dh, 0ah, "$"

.code

multiply macro reg
    push dx
    xor dx, dx
    imul reg
    pop dx
endm

division macro reg
    push dx
    xor dx, dx
    idiv reg
    pop dx
endm

main proc
    mov ax, @data
    mov ds, ax

    lea dx, newRangeMsg     ;Print new range message
    mov ah, 09h
    int 21h

input_lower_bound:
    lea dx, buffer  ;Input lower bound of new range
    mov ah, 0ah
    int 21h
    
    call check_input
    cmp di, 1
    je input_lower_bound
    
    call stringToNumber
    call show_out_of_range
    cmp di, 1
    je input_lower_bound
    
    mov a, ax
    
    lea dx, newLine
    mov ah, 09h
    int 21h
    
input_upper_bound:
    lea dx, buffer  ;Input upper bound of new range
    mov ah, 0ah
    int 21h
    
    call check_input
    cmp di, 1
    je input_upper_bound
    
    call stringToNumber
    call show_out_of_range
    cmp di, 1
    je input_upper_bound
    
    cmp ax, a
    jg continue
    
    lea dx, upperLowerThanLowerMsg
    mov ah, 09h
    int 21h
    
    jmp input_upper_bound
    
continue:
    mov b, ax

    xor cx, cx
    mov cl, maxArraySize
    lea si, array + 2
    
input_loop:
    lea dx, newElementMsg
    mov ah, 09h
    int 21h

    lea dx, buffer
    mov ah, 0ah
    int 21h

    push si
    lea si, buffer + 2
    cmp [si], 'b'       ;Check if user want to stop filling the array
    je stop_input
    
    push cx
    call check_input
    pop cx
    pop si
    cmp di, 1
    je input_loop
    
    call stringToNumber
    call show_out_of_range
    cmp di, 1
    je input_loop
    
    
    mov [si], ax
    
    push cx
    xor cx, cx
    mov cl, b.array[1]      ;Get size of array
    inc cx
    mov b.array[1], cl
    pop cx
    
    add si, 2
    
loop input_loop

jmp algorithm

arraySizeError:
    lea dx, arraySizeErrorMsg
    mov ah, 09h
    int 21h
    jmp input_loop

stop_input:
    pop si
    cmp cx, 97
    jg arraySizeError

; -------------- Main Logic ---------------

algorithm:
    lea si, array + 2
    mov ax, [si]
    mov min_old, ax
    mov max_old, ax
    xor cx, cx
    mov cl, b.array[1]
    
find_min_max:
    mov ax, [si]
    cmp ax, min_old
    jge skip_min
    mov min_old, ax
    
skip_min:
    cmp ax, max_old
    jle skip_max
    mov max_old, ax
    
skip_max:
    add si, 2
    loop find_min_max
    
    ; Check if all array elements are equal
    mov ax, min_old
    cmp ax, max_old
    jne default                 ; If min_old and max_old are different, proceed to standard transformation

    ; If all elements are equal, set all to the middle of the new range
    mov ax, a                   ; Load lower bound of new range into AX
    add ax, b                   ; AX = a + b
    sar ax, 1                   ; AX = (a + b) / 2 (shift right for division by 2)

    ; Assign the midpoint value to all array elements
    lea si, array + 2           ; Point SI to the start of the array (skip size element)
    mov cl, b.array[1]          ; Load the number of elements in CL

fill_with_middle:
    mov [si], ax                ; Set the current element to the midpoint
    add si, 2                   ; Move to the next element
    loop fill_with_middle       ; Repeat for all elements

    call outputArray            ; Skip transformation and go to array output
    jmp exit_program

default:
    mov ax, b
    sub ax, a
    mov bx, ax          ;difference a - b

    mov ax, max_old
    sub ax, min_old
    mov dx, ax          ;difference max_old - min_old

    lea si, array + 2
    mov cl, b.array[1]
    mov di, 10
    
transform_loop:
    mov ax, [si]
    sub ax, min_old
    
	;multiply di
	multiply bx
	
	push bx
	mov bx, dx
	
	push dx
    
    ;division bx
    xor dx, dx
    idiv bx
    
    ;multiply di
    
    push ax
    mov ax, dx
    xor dx, dx
    imul di
    idiv bx
    
    ;push cx
    
rounding:
    ;call get_last_digit
;    pop ax
;    division di
    cmp al, 5
    pop ax
    jl cont_transf
    
    
    cmp ax, 0
    jl hui
    inc ax
    jmp cont_transf
    
hui:
    dec ax    
    
cont_transf:
    pop dx
    pop bx
    
    add ax, a
    
    mov [si], ax

    add si, 2
loop transform_loop

    call outputArray
    
exit_program:
    mov ah, 4ch
    int 21h
    
main endp

; ------------ Functions block ---------------

show_out_of_range proc
    cmp ax, -127
    jl oor
    cmp ax, 127
    jg oor
    
    ret
    
oor:
    push ax
    mov di, 1
    lea dx, outOfRangeMsg
    mov ah, 09h
    int 21h
    pop ax
    ret
show_out_of_range endp

stringToNumber proc
    push si
    push cx
    lea si, buffer + 2
    xor cx, cx
    mov cl, [si-1]
    mov ax, 0
    
    xor bx, bx
    
    mov al, [si]
    cmp al, '-'
    jne positive_number
    inc si
    dec cl
    mov bx, 1    


positive_number:
    push bx
    xor ax, ax
convert_loop:
    mov bl, [si]

    sub bl, '0'
    push bx
    mov bx, 10
    imul bx
    pop bx
    add ax, bx

    inc si
loop convert_loop

    pop bx
    cmp bx, 1
    jne end_convert
    neg ax

end_convert:
    pop cx
    pop si
    ret
    
stringToNumber endp

check_input proc
    xor di, di
    xor cx, cx
    lea si, buffer + 2
    mov cl, [si-1]
    cmp cx, 0                 ; Проверка на пустую строку
    je empty_message

    mov al, [si]               ; Load the first character
    cmp al, '-'                ; Check if the first character is a minus sign
    jne check_digits           ; If not, go to digit-checking loop

    ; If the first character is '-', allow it, but ensure no other minus signs
    inc si                     ; Move to the next character
    dec cx                     ; Decrease the count of remaining characters
    cmp cx, 0                  ; Check if there's anything left after the minus sign
    je print_NotNumber_error   ; If nothing after '-', show error

check_digits:
    ; Loop through each character to ensure all are digits
check_loop:
    lodsb                      ; Load next character into AL
    cmp al, '-'                ; Check if there's an additional minus sign
    je print_NotNumber_error   ; If another minus is found, show error
    cmp al, '0'                ; Check if it's below '0'
    jl print_NotNumber_error   ; If below '0', show error
    cmp al, '9'                ; Check if it's above '9'
    jg print_NotNumber_error   ; If above '9', show error

    loop check_loop            ; Decrement CX and repeat until CX = 0

    ret                        ; Return with di = 0 (no error)

print_NotNumber_error:
    mov di, 1                  ; Set error flag
    lea dx, foundNotNumberError
    mov ah, 09h
    int 21h
    ret

empty_message:
    mov di, 1                  ; Set error flag
    lea dx, emptyInputError
    mov ah, 09h
    int 21h
    ret
    
check_input endp

;get_first_digit proc
;    cmp dx, 0
;    jl  end_function
;
;loop_divide:
;    cmp dx, 10
;    jl  end_function
;
;    mov ax, dx
;    xor dx, dx
;    mov cx, 10
;    div cx
;    mov dx, ax
;    jmp loop_divide
;
;end_function:
;    ret
;
;get_first_digit endp


get_last_digit proc
    xor dx, dx
    idiv di
    
    ret
get_last_digit endp


outputArray proc
    
    lea dx, newLine
    mov ah, 09h
    int 21h
    
    lea dx, modifiedArrayMsg
    mov ah, 09h
    int 21h
    
    ; Initial setup for displaying the array
    lea si, array + 2
    xor cx, cx
    mov cl, b.array[1]
    
display_loop:
    mov ax, [si]
    push cx
    
    ;Check for negative number
    cmp ax, 0
    jge display_positive
    push ax
    lea dx, minusSign
    mov ah, 09h
    int 21h
    pop ax
    neg ax                  
    
display_positive:
    ; Convert AX (the number) to a decimal string in buffer
    lea di, buffer + 6        ; Set DI to buffer + 1 (skip the first byte which stores string length)
    mov bx, 10                ; We will divide by 10 to convert to decimal
    xor cx, cx                ; CX will hold the digit count

convert_to_string:
    xor dx, dx                ; Clear DX before dividing
    div bx                    ; AX = AX / 10, remainder in DX (holds current digit)
    add dl, '0'               ; Convert remainder to ASCII character
    dec di                    ; Move back in buffer to store digit
    mov [di], dl              ; Store digit in buffer
    inc cx                    ; Increment digit count
    test ax, ax               ; Check if AX is 0 (no more digits left)
    jnz convert_to_string     ; If not, continue dividing

    mov [buffer], cl          ; Set string length at the start of buffer
    lea dx, di                ; Load buffer address in DX
    mov ah, 09h               
    int 21h
    
    ; Print newline after each number
    lea dx, newLine
    mov ah, 09h
    int 21h
    
    pop cx
    add si, 2
    loop display_loop
    
    ret
outputArray endp

end main
