export type JwtTokenClaims = { 
    sub: string; 
    username: string; 
    exp?: number 
    nbf?: number
};