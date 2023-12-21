section .text
global gauss_solver

gauss_solver:
    ; RDI - wskaŸnik do macierzy
    ; RCX - rozmiar macierzy (iloœæ wierszy)
    
    xor rax, rax  ; RAX bêdzie u¿ywany jako indeks wiersza

    ; Przekszta³æ macierz do postaci trójk¹tnej górnej
    outer_loop:
        ; Tutaj implementuj kod przekszta³caj¹cy macierz

        inc rax
        cmp rax, rcx
        jl outer_loop  ; Kontynuuj pêtlê do momentu przejœcia przez wszystkie wiersze

    ; Rozwi¹zanie uk³adu równañ z macierz¹ trójk¹tn¹ górn¹
    xor rax, rax  ; RAX bêdzie u¿ywany jako indeks wiersza

    inner_loop:
        ; Tutaj implementuj kod rozwi¹zuj¹cy uk³ad równañ

        inc rax
        cmp rax, rcx
        jl inner_loop  ; Kontynuuj pêtlê do momentu przejœcia przez wszystkie wiersze

    ret

end