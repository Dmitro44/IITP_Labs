.model small     
.stack 100h
.data
message db "assembler",0dh,0ah,'$'
.code

start:        
    mov ax,@data 
    mov ds,ax  
    
    mov ah,09h
    mov dx,offset message
    int 21h    
    
    mov ax,4c00h
    int 21h       
end start