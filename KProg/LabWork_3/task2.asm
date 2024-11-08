;Create a program that adds one given word before target word in a string
;write it for tasm and tlink
 
.model small
.stack 100h

.data
    input_string db 255, ?, 256 dup('$')
    target_word db 50, ?, 51 dup('$')
    word_to_add db 50, ?, 51 dup('$')
    prompt db 0dh, 0ah, "enter string: $"
    target db 0dh, 0ah, "enter target word: $"
    mess_to_add db 0dh, 0ah, "enter word to add: $"
    foundMes db 0dh, 0ah, "target word was found$"
    notFoundMsg db 0dh, 0ah, "target word was not found$"
    foundNumberError db 0dh, 0ah, "number was found in input string$"
    foundDollarError db 0dh, 0ah, "dollar sign was found in input string$"
    emptyInputError db 0dh, 0ah, "empty input string$"
    modifiedString db 0dh, 0ah, "modified string:$"
    newline db 0dh, 0ah, "$"
 
.code

main proc
    mov ax, @data
    mov ds, ax
    mov es, ax
 
    xor cx, cx
get_input_string:
    mov di, 0
    ;Print prompt
    lea dx, prompt
    mov ah, 09h
    int 21h
 
    ;Read the string from buffer
    lea dx, input_string
    mov ah, 0ah
    int 21h
 
    lea si, input_string + 2
    mov cl, [si-1]      ;store length of input_string
    call check_input
 
    cmp di, 1
    je get_input_string
 
get_target_word:
    mov di, 0
    ;Print target word suggestion
    lea dx, target
    mov ah, 09h
    int 21h
 
    ;Read target word from buffer
    lea dx, target_word
    mov ah, 0ah
    int 21h
 
    lea si, target_word + 2
    mov cl, [si-1]              ;store length of input_string
    call check_input
 
    cmp di, 1 
    je get_target_word
 
get_word_to_add:
    mov di, 0
    ;Print message for input word that need to add 
    lea dx, mess_to_add
    mov ah, 09h
    int 21h
 
    ;Read word to add from buffer 
    lea dx, word_to_add
    mov ah, 0ah
    int 21h
 
    lea si, word_to_add + 2
    mov cl, [si-1]              ;store length of input_string
    call check_input
 
    cmp di, 1
    je get_word_to_add
 
    lea dx, newline
    mov ah, 09h
    int 21h
                
    ;xor dx, dx            
    ;sub al, dl  ;???  
                ;???
    ;xor cx, cx  ;???
    ;mov cl, al  ;???
    ;inc cx      ;???
 
    call insert_word
    ;cmp bx, 1
    ;je end_of_program

show_new_string:
    lea dx, newline
    mov ah, 09h
    int 21h
    
    lea dx, modifiedString
    mov ah, 09h
    int 21h
    
    lea dx, newline
    mov ah, 09h
    int 21h
 
    ; Print the modified input_string
    lea dx, input_string + 3
    mov ah, 09h
    int 21h
    
end_of_program:
    ; End of main procedure
    mov ax, 4C00h
    int 21h
    
main endp
 
check_input proc
    cmp cx, 0
    je empty_message
check_loop:
    lodsb
    cmp al, '0'
    jl check_dollar
    cmp al, '9'
    jle print_number_error
check_dollar:
    cmp al, '$'
    je print_dollar_error
    loop check_loop  ; Decrement CX and repeat if CX != 0
    
    cmp cl, 0
    je end_check
 
print_number_error:
    mov di, 1
    lea dx, foundNumberError
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
 
is_that_a_word proc
    jnz return 
    
has_space_after:
    mov al, [si]
    cmp al, ' '
    je has_space_before
    
    
    add si, cx
return:
    ret
    
has_space_before:
    mov cl, target_word[1]
    add cx, 1
    sub si, cx
    mov al, [si]
    cmp al, ' '
    je word_found
    
    
is_that_a_word endp

; Finds target_word in input_string and sets si register to start of target_word
find_word proc
    
searching:
    push cx
    push di         
    xor cx, cx
    mov cl, target_word[1]
    repe cmpsb
    call is_that_a_word
    pop di
    pop cx
    ;cmp cl, target_word[1]
    ;jb not_found_word
        
loop searching
 
not_found_word:
    cmp bx, 2
    je exit
    
    mov bx, 1
    lea dx, notFoundMsg
    mov ah, 09h
    int 21h
    
exit:
    mov bx, 1
    ret
    
word_found:
    pop bx
    pop di
    pop cx
    xor bx, bx
 
    mov bx, 2
    lea dx, foundMes
    mov ah, 09h
    int 21h
    
    ret
 
find_word endp
 
insert_word proc
    mov cl, input_string[1]
    mov si, cx
    mov [si+2], ' '
    
    mov di, cx
    add di, 3
    add si, 2
    inc cx
    
    std
    rep movsb
    
    mov si, offset input_string + 2
    mov [si], ' '
    
    mov cl, input_string[1]
    inc cx
    mov input_string[1], cl
    cld
    ; Now input_string prepared for searching target_word correctly
    
    mov di, offset target_word + 2
    mov cl, input_string[1] ; Length of input_string
    
inserting:
    mov di, offset target_word + 2
    
    call find_word
    cmp bx, 1
    je end_inserting
    
    ; SI now points to start of the target_word in input_string
    push si
    mov si, offset input_string
    xor ax, ax
    mov al, [si+1]
    mov di, ax
    pop si
    xor cx, cx
    mov cl, word_to_add[1]
    add di, cx
    add di, 3 ; Now di points to the end of shifted input_string
    
    mov cx, ax
    sub cx, si
    add cx, 3 ; CX stores number of symbols to be shifted
    
    push si
    mov si, ax
    add si, 2
    
    std
    rep movsb ; Shifting input_string
    
    pop si
    mov di, si
    inc di
    mov si, offset word_to_add + 2
    mov cl, word_to_add[1]
    
    cld
    rep movsb ; Copying word_to_add to input_string
    
    mov si, di ; SI points to the end of word_to_add in input_string
    mov cl, target_word[1]
    add si, cx ; SI points to the end of target_word in input_string
    
    mov cl, word_to_add[1]
    add cx, 2
    add cl, input_string[1]
    mov input_string[1], cl
    
    mov cl, input_string[1]
    sub cx, si
    cmp target_word[1], cl
    jbe inserting
    
end_inserting:
    ret
    
insert_word endp
 
end