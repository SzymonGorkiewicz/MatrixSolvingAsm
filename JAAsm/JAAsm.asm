.data
    columns dq 0

.code


MyProc1 proc
	; rcx pointer na tablice
	; rdx liczba wierszy
    ; rbx liczba kolumn
    ; mov r12, r9 ; d³ugosc macierzy
   
	xor r8, r8  ; i = 0
    xor r9, r9
    
loop_i_start:
    cmp r8, rdx       ; Porównaj i z rows
    jge loop_i_end    ; Jeœli i >= rows, zakoñcz pêtlê i
    mov r10, rbx
    push r8
    mov rax, r8
    imul rax, rbx
    add rax, r8
    shl rax, 3
    add rax, rcx
    movd xmm0, qword ptr [rax] ; element diagonalny
    pop r8
    vbroadcastsd ymm0, xmm0
    

           ; j = i
    cmp rbx, 4
    jl handle_remainder
    sub r10, 4
    loop_j_start:
        
        mov rax, 0
        add rax, r9
        shl rax, 3
        add rax, rcx
        
        vmovupd ymm1, qword ptr [rax] ; zapisanie 4 wartosci wiersza do ymm1
        vdivpd ymm1, ymm1, ymm0 ; dzielenie przez element diagonalny

        vmovupd qword ptr [rax], ymm1

        add r9, 4       ; j++
        sub r10, 4
        jg loop_j_start
        jmp handle_remainder
        
    loop_j_end:

    ; Eliminacja Gaussa dla wierszy poni¿ej i-tego
    lea r10, [r8 + 1] ; k = i + 1
    loop_k_start:
        cmp r10, rdx  ; Porównaj k z rows
        jge loop_k_end ; Jeœli k >= rows, zakoñcz pêtlê k

        push r10
        mov rax, r10
        imul rax, rbx
        add rax, r8
        shl rax, 3
        add rax, rcx
        movd xmm2, qword ptr [rax] ; factor
        pop r10
        
        mov r11, r8   ; j = i
        loop_kj_start:
            cmp r11, rbx  ; Porównaj j z columns
            jge loop_kj_end ; Jeœli j >= columns, zakoñcz pêtlê j wewn¹trz k

            ; Wykonaj flatMatrix[k * columns + j] -= factor * flatMatrix[i * columns + j]
            push r10
            push r8
            mov rax, r8
            imul rax, rbx
            add rax, r11
            shl rax, 3
            add rax, rcx
            movd xmm3, qword ptr [rax]
            mulsd xmm3, xmm2

            mov rax, r10
            imul rax, rbx
            add rax, r11
            shl rax, 3
            add rax, rcx
            movd xmm4, qword ptr [rax]

            subsd xmm4, xmm3
            movsd qword ptr [rax], xmm4
            pop r8
            pop r10
            

            inc r11   ; j++
            jmp loop_kj_start
        loop_kj_end:

        inc r10   ; k++
        jmp loop_k_start
    loop_k_end:
    cmp rbx, 4
    jl handle_remainder

    inc r8    ; i++
    jmp loop_i_start
loop_i_end:
  
    mov r10, [rsp + 40]

    mov r8, rdx
    dec r8 ; i = rows - 1
    jmp rozwiazywanie

rozwiazywanie:
    cmp r8, 0
    jl end_rozwiazywanie

    mov rax, 0
    add rax, r8
    imul rax, rbx
    add rax, rbx
    sub rax, 1
    shl rax, 3
    add rax, rcx
    

    movd xmm0, qword ptr [rax] ; wpisanie do solutions[] wyników od konca tablicy

    mov rax, 0
    add rax, r8
    shl rax, 3
    add rax, r10

    movd qword ptr [rax], xmm0 ; solutions[i]
    

    mov r9, r8  ; j = i + 1
    inc r9
    rozwiazywanie2:
        cmp r9, rdx  ; Porównaj j z rows
        jge end_rozwiazywanie2 ; Jeœli j >= rows, zakoñcz pêtlê j

        mov rax, 0
        add rax, r8
        imul rax, rbx
        add rax, r9
        shl rax, 3
        add rax, rcx

        movd xmm2, qword ptr [rax] ; Wczytaj flattenedMatrix[i * cols + j] do xmm2
        
        mov rax, 0
        add rax, r9
        shl rax, 3
        add rax, r10

        mulsd xmm2, qword ptr [rax]

        subsd xmm0, xmm2

        mov rax, 0
        add rax, r8
        shl rax, 3
        add rax, r10
        movd qword ptr [rax], xmm0
       
        inc r9 ; j++
        jmp rozwiazywanie2

    end_rozwiazywanie2:
        dec r8 ; i--
        jmp rozwiazywanie

end_rozwiazywanie:
    ret



handle_remainder:
    handle_inner_loop:
        cmp r9, rbx
        jge loop_j_end

        push r8
        mov rax, r9
        imul r8, rbx
        add rax, r8
        shl rax, 3
        add rax, rcx
        movd xmm1, qword ptr [rax]
        pop r8

        divsd xmm1, xmm0
        movsd qword ptr [rax], xmm1

        inc r9
        jmp handle_inner_loop
    
MyProc1 endp

END