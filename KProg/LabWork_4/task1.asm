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
    buffer db 6, ?, 7 dup('$')
    newRangeMsg db 0dh, 0ah, "Enter lower and upper bound of new range: ", 0dh, 0ah, "$"
    newElementMsg db 0dh, 0ah, "Enter new element (-127...127), 99 elements max (enter b if you want to stop): ", 0dh, 0ah, "$"
    foundNotNumberError db 0dh, 0ah, "Enter a number please!", 0dh, 0ah, "$"
    foundDollarError db 0dh, 0ah, "Dollar sigh was found, enter a number please!", 0dh, 0ah, "$"
    emptyInputError db 0dh, 0ah, "Empty input, enter a number please!", 0dh, 0ah, "$"
    upperLowerThanLowerMsg db 0dh, 0ah, "Upper bound is lower than or equal to lower bound, this is incorrect!", 0dh, 0ah, "$"
    arraySizeErrorMsg db 0dh, 0ah, "Array should have at least 2 elements", 0dh, 0ah, "$"
    modifiedArrayMsg db 0dh, 0ah, "Modified array:", 0dh, 0ah, "$"
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
    
	multiply di
	multiply bx
	
	push bx
	mov bx, dx
	
	push dx
    
    division bx
    
    push ax
    ;push cx
    
rounding:
    call get_last_digit
    pop ax
    division di
    cmp dl, 5
    jl cont_transf
    
    
    inc ax    
    
cont_transf:
    pop dx
    pop bx
    
    add ax, a
    
    mov [si], ax

    add si, 2
loop transform_loop

    call outputArray

    mov ah, 4ch
    int 21h
    
main endp

; ------------ Functions block ---------------

stringToNumber proc
    push si
    push cx
    lea si, buffer + 2
    xor cx, cx
    mov cl, [si-1]
    mov ax, 0

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

check_loop:
    lodsb                      ; Загрузка символа в AL
    cmp al, '0'                ; Проверка, меньше ли символ '0'
    jl check_dollar
    cmp al, '9'                ; Проверка, больше ли символ '9'
    jle continue_check

check_dollar:
    cmp al, '$'                ; Проверка на символ '$'
    je print_dollar_error      ; Если найден '$', перейти к выводу ошибки

    ; Если символ не число и не '$', то это ошибка
    jmp print_NotNumber_error

continue_check:
    loop check_loop            ; Уменьшение CX и повторение цикла, если CX != 0

    cmp cx, 0
    je end_check

print_NotNumber_error:
    mov di, 1
    lea dx, foundNotNumberError
    mov ah, 09h
    int 21h
    ret

print_dollar_error:
    mov di, 1
    lea dx, foundDollarError
    mov ah, 09h
    int 21h
    ret

empty_message:
    mov di, 1
    lea dx, emptyInputError
    mov ah, 09h
    int 21h
    ret

end_check:
    ret
check_input endp

get_first_digit proc
    cmp dx, 0
    jl  end_function

loop_divide:
    cmp dx, 10
    jl  end_function

    mov ax, dx
    xor dx, dx
    mov cx, 10
    div cx
    mov dx, ax
    jmp loop_divide

end_function:
    ret

get_first_digit endp


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
    lea si, array + 2         ; Start from the first element in array (skip size)
    xor cx, cx
    mov cl, b.array[1]        ; Number of elements to display
    
display_loop:
    mov ax, [si]              ; Load current array element into AX
    push cx                   ; Save AX for the integer to string conversion
    
    ; Convert AX (the number) to a decimal string in buffer
    lea di, buffer + 7        ; Set DI to buffer + 1 (skip the first byte which stores string length)
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

    ;inc di
    mov [buffer], cl          ; Set string length at the start of buffer
    lea dx, di            ; Load buffer address in DX
    mov ah, 09h               ; DOS function to display string
    int 21h                   ; Call DOS interrupt to print the number
    
    ; Print newline after each number
    lea dx, newLine           ; Address of newline string
    mov ah, 09h               ; DOS function to display string
    int 21h                   ; Call DOS interrupt to print the newline
    
    pop cx                    ; Restore AX to proceed with the next element
    add si, 2                 ; Move to the next array element
    loop display_loop         ; Repeat for all elements in array
    
    ret
outputArray endp

end main
