	org $8000
start:
	ldx #$8200
	ldy #0

loop:
	ldaa 0,X
	ldab #8 

check_bits:
	lsra
	bcc zero_bit
	decb
	bne check_bits

	inx
	cpx #$821f
	bne loop

	bra done

zero_bit:
	iny
	decb
	bne check_bits

	inx
	cpx #$821f
	bne loop

done:
	swi  