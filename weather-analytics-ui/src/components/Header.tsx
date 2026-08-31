interface HeaderProps {
    darkMode: boolean;
    onToggleDarkMode: () => void;
    onRefresh: () => void;
}

function Header({
    darkMode,
    onToggleDarkMode,
    onRefresh,
}: HeaderProps) {
    return (
        <header className="header">
            <div className="brand">
                <div className="brand-icon">WA</div>

                <div>
                    <h1>Weather Analytics</h1>
                    <p>Global weather comfort dashboard</p>
                </div>
            </div>

            <div className="header-actions">
                <button
                    className="theme-button"
                    onClick={onToggleDarkMode}
                >
                    {darkMode ? "Light mode" : "Dark mode"}
                </button>

                <button
                    className="refresh-button"
                    onClick={onRefresh}
                >
                    Refresh
                </button>
            </div>
        </header>
    );
}

export default Header;