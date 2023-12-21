section .text
global gauss_solver

gauss_solver:
    ; RDI - wska�nik do macierzy
    ; RCX - rozmiar macierzy (ilo�� wierszy)
    
    xor rax, rax  ; RAX b�dzie u�ywany jako indeks wiersza

    ; Przekszta�� macierz do postaci tr�jk�tnej g�rnej
    outer_loop:
        ; Tutaj implementuj kod przekszta�caj�cy macierz

        inc rax
        cmp rax, rcx
        jl outer_loop  ; Kontynuuj p�tl� do momentu przej�cia przez wszystkie wiersze

    ; Rozwi�zanie uk�adu r�wna� z macierz� tr�jk�tn� g�rn�
    xor rax, rax  ; RAX b�dzie u�ywany jako indeks wiersza

    inner_loop:
        ; Tutaj implementuj kod rozwi�zuj�cy uk�ad r�wna�

        inc rax
        cmp rax, rcx
        jl inner_loop  ; Kontynuuj p�tl� do momentu przej�cia przez wszystkie wiersze

    ret

end